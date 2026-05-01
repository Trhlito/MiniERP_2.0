using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MiniERP.API.DTOs.Auth;
using MiniERP.API.Services.Interfaces;
using MiniERP.Data;
using MiniERP.Data.Entities.Auth;

namespace MiniERP.API.Services.Implementations;

// Služba řeší přihlášení, refresh tokeny a audit autentizačních událostí
public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _dbContext;
    private readonly IConfiguration _configuration;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext dbContext,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _dbContext = dbContext;
        _configuration = configuration;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, string? ipAddress)
    {
        var user = request.UserNameOrEmail.Contains('@')
            ? await _userManager.FindByEmailAsync(request.UserNameOrEmail)
            : await _userManager.FindByNameAsync(request.UserNameOrEmail);

        // Stejná chyba pro neexistující i neaktivní účet neprozrazuje stav účtu
        if (user is null || !user.IsActive)
        {
            await WriteAuthAuditLogAsync(
                null,
                request.UserNameOrEmail,
                "LOGIN_FAILED",
                false,
                ipAddress,
                "Invalid user or inactive account");

            throw new UnauthorizedAccessException("Invalid login credentials.");
        }

        var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);

        // Odpověď zůstává obecná, aby nešlo poznat rozdíl mezi špatným heslem a účtem
        if (!passwordValid)
        {
            await WriteAuthAuditLogAsync(
                user.Id,
                user.UserName,
                "LOGIN_FAILED",
                false,
                ipAddress,
                "Invalid password");

            throw new UnauthorizedAccessException("Invalid login credentials.");
        }

        var response = await CreateTokenResponseAsync(user);
        var refreshToken = CreateRefreshToken(user.Id, ipAddress);

        _dbContext.RefreshTokens.Add(refreshToken);
        await _dbContext.SaveChangesAsync();

        response.RefreshToken = refreshToken.Token;

        await WriteAuthAuditLogAsync(
            user.Id,
            user.UserName,
            "LOGIN_SUCCESS",
            true,
            ipAddress);

        return response;
    }

    public async Task<LoginResponse> RefreshAsync(RefreshTokenRequest request, string? ipAddress)
    {
        var existingToken = await _dbContext.RefreshTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Token == request.RefreshToken);

        if (existingToken is null || !existingToken.IsActive)
        {
            await WriteAuthAuditLogAsync(
                null,
                null,
                "REFRESH_FAILED",
                false,
                ipAddress,
                "Invalid refresh token");

            throw new UnauthorizedAccessException("Invalid refresh token.");
        }

        if (!existingToken.User.IsActive)
        {
            await WriteAuthAuditLogAsync(
                existingToken.UserId,
                existingToken.User.UserName,
                "REFRESH_FAILED",
                false,
                ipAddress,
                "Inactive user");

            throw new UnauthorizedAccessException("User is inactive.");
        }

        var newRefreshToken = CreateRefreshToken(existingToken.UserId, ipAddress);

        // Refresh token rotation: použitý token se zneplatní a nahradí novým.
        existingToken.RevokedAt = DateTime.UtcNow;
        existingToken.RevokedByIp = ipAddress;
        existingToken.ReplacedByToken = newRefreshToken.Token;

        _dbContext.RefreshTokens.Add(newRefreshToken);
        await _dbContext.SaveChangesAsync();

        var response = await CreateTokenResponseAsync(existingToken.User);
        response.RefreshToken = newRefreshToken.Token;

        await WriteAuthAuditLogAsync(
            existingToken.UserId,
            existingToken.User.UserName,
            "REFRESH_SUCCESS",
            true,
            ipAddress);

        return response;
    }

    public async Task LogoutAsync(LogoutRequest request, string? ipAddress)
    {
        var existingToken = await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == request.RefreshToken);

        // Logout je idempotentní, neexistující token není potřeba dál řešit.
        if (existingToken is null)
        {
            return;
        }

        if (existingToken.IsRevoked)
        {
            return;
        }

        existingToken.RevokedAt = DateTime.UtcNow;
        existingToken.RevokedByIp = ipAddress;

        await _dbContext.SaveChangesAsync();

        await WriteAuthAuditLogAsync(
            existingToken.UserId,
            null,
            "LOGOUT",
            true,
            ipAddress);
    }

    public async Task<CurrentUserResponse?> GetCurrentUserAsync(ClaimsPrincipal principal)
    {
        var userIdValue = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out var userId))
        {
            return null;
        }

        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null || !user.IsActive)
        {
            return null;
        }

        var roles = await _userManager.GetRolesAsync(user);

        return new CurrentUserResponse
        {
            UserId = user.Id,
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Roles = roles.ToList()
        };
    }

    private async Task<LoginResponse> CreateTokenResponseAsync(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var jwtSection = _configuration.GetSection("Jwt");

        var expiresAt = DateTime.UtcNow.AddMinutes(
            int.Parse(jwtSection["ExpirationMinutes"]!));

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSection["Key"]!));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSection["Issuer"],
            audience: jwtSection["Audience"],
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return new LoginResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = expiresAt,
            UserId = user.Id,
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            Roles = roles.ToList()
        };
    }

    private RefreshToken CreateRefreshToken(int userId, string? ipAddress)
    {
        var jwtSection = _configuration.GetSection("Jwt");
        var randomBytes = RandomNumberGenerator.GetBytes(64);

        return new RefreshToken
        {
            UserId = userId,
            Token = Convert.ToBase64String(randomBytes),
            ExpiresAt = DateTime.UtcNow.AddDays(
                int.Parse(jwtSection["RefreshTokenExpirationDays"]!)),
            CreatedAt = DateTime.UtcNow,
            CreatedByIp = ipAddress
        };
    }

    private async Task WriteAuthAuditLogAsync(
        int? userId,
        string? userName,
        string actionType,
        bool success,
        string? ipAddress,
        string? failureReason = null)
    {
        var log = new AuthAuditLog
        {
            UserId = userId,
            UserName = userName,
            ActionType = actionType,
            Success = success,
            IpAddress = ipAddress,
            FailureReason = failureReason,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.AuthAuditLogs.Add(log);
        await _dbContext.SaveChangesAsync();
    }
}