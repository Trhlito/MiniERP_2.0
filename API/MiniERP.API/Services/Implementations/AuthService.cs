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
using System.Security.Claims;

namespace MiniERP.API.Services.Implementations;

// Služba zajišťuje přihlášení a práci s tokeny
public class AuthService : IAuthService
{
    // Správa uživatelů přes ASP.NET Identity
    private readonly UserManager<ApplicationUser> _userManager;

    // Databázový kontext aplikace
    private readonly ApplicationDbContext _dbContext;

    // Konfigurace aplikace
    private readonly IConfiguration _configuration;

    // Konstruktor služby
    public AuthService(
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext dbContext,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _dbContext = dbContext;
        _configuration = configuration;
    }

    // Metoda přihlásí uživatele a vrátí tokeny
    public async Task<LoginResponse> LoginAsync(LoginRequest request, string? ipAddress)
    {
        // Vyhledání uživatele podle e-mailu nebo uživatelského jména
        var user = request.UserNameOrEmail.Contains('@')
            ? await _userManager.FindByEmailAsync(request.UserNameOrEmail)
            : await _userManager.FindByNameAsync(request.UserNameOrEmail);

        // Obecná chyba při neplatném uživateli
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

        // Ověření hesla uživatele
        var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);

        // Obecná chyba při neplatném hesle
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

        // Vytvoření odpovědi s access tokenem
        var response = await CreateTokenResponseAsync(user);

        // Vytvoření refresh tokenu
        var refreshToken = CreateRefreshToken(user.Id, ipAddress);

        // Uložení refresh tokenu do databáze
        _dbContext.RefreshTokens.Add(refreshToken);
        await _dbContext.SaveChangesAsync();

        // Doplnění refresh tokenu do odpovědi
        response.RefreshToken = refreshToken.Token;
        await WriteAuthAuditLogAsync(
            user.Id,
            user.UserName,
            "LOGIN_SUCCESS",
            true,
            ipAddress);

        // Vrácení přihlašovací odpovědi
        return response;
    }

    // Metoda obnoví JWT token pomocí refresh tokenu
    public async Task<LoginResponse> RefreshAsync(RefreshTokenRequest request, string? ipAddress)
    {
        // Vyhledání refresh tokenu v databázi
        var existingToken = await _dbContext.RefreshTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Token == request.RefreshToken);

        // Kontrola existence a platnosti refresh tokenu
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

        // Kontrola aktivního uživatele
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

        // Vytvoření nového refresh tokenu
        var newRefreshToken = CreateRefreshToken(existingToken.UserId, ipAddress);

        // Zneplatnění starého refresh tokenu
        existingToken.RevokedAt = DateTime.UtcNow;
        existingToken.RevokedByIp = ipAddress;
        existingToken.ReplacedByToken = newRefreshToken.Token;

        // Uložení nového refresh tokenu
        _dbContext.RefreshTokens.Add(newRefreshToken);
        await _dbContext.SaveChangesAsync();

        // Vytvoření nové odpovědi s access tokenem
        var response = await CreateTokenResponseAsync(existingToken.User);

        // Doplnění nového refresh tokenu do odpovědi
        response.RefreshToken = newRefreshToken.Token;
        await WriteAuthAuditLogAsync(
            existingToken.UserId,
            existingToken.User.UserName,
            "REFRESH_SUCCESS",
            true,
            ipAddress);

        // Vrácení nové token odpovědi
        return response;
    }

    // Metoda odhlásí uživatele zneplatněním refresh tokenu
    public async Task LogoutAsync(LogoutRequest request, string? ipAddress)
    {
        // Vyhledání refresh tokenu
        var existingToken = await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == request.RefreshToken);

        // Neexistující token se bere jako dokončené odhlášení
        if (existingToken is null)
        {
            return;
        }

        // Již zneplatněný token se znovu neupravuje
        if (existingToken.IsRevoked)
        {
            return;
        }

        // Zneplatnění refresh tokenu
        existingToken.RevokedAt = DateTime.UtcNow;
        existingToken.RevokedByIp = ipAddress;

        // Uložení změn
        await _dbContext.SaveChangesAsync();
        await WriteAuthAuditLogAsync(
            existingToken.UserId,
            null,
            "LOGOUT",
            true,
            ipAddress);
    }

    // Metoda vytvoří odpověď s JWT tokenem
    private async Task<LoginResponse> CreateTokenResponseAsync(ApplicationUser user)
    {
        // Načtení rolí uživatele
        var roles = await _userManager.GetRolesAsync(user);

        // Načtení JWT konfigurace
        var jwtSection = _configuration.GetSection("Jwt");

        // Výpočet expirace tokenu
        var expiresAt = DateTime.UtcNow.AddMinutes(
            int.Parse(jwtSection["ExpirationMinutes"]!));

        // Příprava claims pro JWT token
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Přidání rolí do claims
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        // Vytvoření podpisového klíče
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSection["Key"]!));

        // Vytvoření podepisovacích údajů
        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        // Vytvoření JWT tokenu
        var token = new JwtSecurityToken(
            issuer: jwtSection["Issuer"],
            audience: jwtSection["Audience"],
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        // Vrácení token odpovědi
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

    // Metoda vytvoří nový refresh token
    private RefreshToken CreateRefreshToken(int userId, string? ipAddress)
    {
        // Načtení JWT konfigurace
        var jwtSection = _configuration.GetSection("Jwt");

        // Vygenerování bezpečné náhodné hodnoty tokenu
        var randomBytes = RandomNumberGenerator.GetBytes(64);

        // Vrácení nové entity refresh tokenu
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

    // Metoda zapíše autentizační událost do audit logu
    private async Task WriteAuthAuditLogAsync(
        int? userId,
        string? userName,
        string actionType,
        bool success,
        string? ipAddress,
        string? failureReason = null)
    {
        // Vytvoření auditního záznamu
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

        // Uložení auditního záznamu
        _dbContext.AuthAuditLogs.Add(log);
        await _dbContext.SaveChangesAsync();

    }

    // Metoda vrátí aktuálně přihlášeného uživatele podle JWT tokenu
    public async Task<CurrentUserResponse?> GetCurrentUserAsync(ClaimsPrincipal principal)
    {
        // Načtení user id z claims
        var userIdValue = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        // Ošetření chybějícího nebo neplatného user id
        if (!int.TryParse(userIdValue, out var userId))
        {
            return null;
        }

        // Vyhledání uživatele podle id
        var user = await _userManager.FindByIdAsync(userId.ToString());

        // Ošetření nenalezeného nebo neaktivního uživatele
        if (user is null || !user.IsActive)
        {
            return null;
        }

        // Načtení rolí uživatele
        var roles = await _userManager.GetRolesAsync(user);

        // Vrácení aktuálně přihlášeného uživatele
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
}