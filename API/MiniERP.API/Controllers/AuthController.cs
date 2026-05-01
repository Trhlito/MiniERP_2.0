using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniERP.API.DTOs.Auth;
using MiniERP.API.Services.Interfaces;

namespace MiniERP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // Endpoint přihlásí uživatele a vrátí tokeny
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        try
        {
            var result = await _authService.LoginAsync(
                request,
                HttpContext.Connection.RemoteIpAddress?.ToString());

            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            // Chyba zůstává obecná, aby API neprozrazovalo detail o účtu nebo hesle.
            return Unauthorized(new
            {
                message = "Invalid login credentials."
            });
        }
    }

    // Endpoint obnoví access token pomocí refresh tokenu
    [HttpPost("refresh")]
    public async Task<ActionResult<LoginResponse>> Refresh(
        RefreshTokenRequest request)
    {
        try
        {
            var result = await _authService.RefreshAsync(
                request,
                HttpContext.Connection.RemoteIpAddress?.ToString());

            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new
            {
                message = "Invalid refresh token."
            });
        }
    }

    // Endpoint odhlásí uživatele zneplatněním refresh tokenu
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(
        LogoutRequest request)
    {
        await _authService.LogoutAsync(
            request,
            HttpContext.Connection.RemoteIpAddress?.ToString());

        return Ok(new
        {
            message = "Logged out successfully."
        });
    }

    // Endpoint vrátí aktuálně přihlášeného uživatele
    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<CurrentUserResponse>> Me()
    {
        var currentUser = await _authService.GetCurrentUserAsync(User);

        if (currentUser is null)
        {
            return Unauthorized();
        }

        return Ok(currentUser);
    }
}