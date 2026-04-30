using Microsoft.AspNetCore.Mvc;
using MiniERP.API.DTOs.Auth;
using MiniERP.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MiniERP.API.Controllers;

// Controller zajišťuje autentizační endpointy
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    // Služba pro autentizaci uživatelů
    private readonly IAuthService _authService;

    // Konstruktor controlleru
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
            // Přihlášení uživatele
            var result = await _authService.LoginAsync(
                request,
                HttpContext.Connection.RemoteIpAddress?.ToString());

            // Vrácení tokenů
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            // Obecná odpověď při neplatném přihlášení
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
            // Obnovení tokenů
            var result = await _authService.RefreshAsync(
                request,
                HttpContext.Connection.RemoteIpAddress?.ToString());

            // Vrácení nových tokenů
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            // Obecná odpověď při neplatném refresh tokenu
            return Unauthorized(new
            {
                message = "Invalid refresh token."
            });
        }
    }

    // Endpoint odhlásí uživatele
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(
        LogoutRequest request)
    {
        // Odhlášení uživatele
        await _authService.LogoutAsync(
            request,
            HttpContext.Connection.RemoteIpAddress?.ToString());

        // Potvrzení odhlášení
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
        // Načtení aktuálně přihlášeného uživatele
        var currentUser = await _authService.GetCurrentUserAsync(User);

        // Ošetření neplatného nebo neaktivního uživatele
        if (currentUser is null)
        {
            return Unauthorized();
        }

        // Vrácení aktuálního uživatele
        return Ok(currentUser);
    }
}