using MiniERP.API.DTOs.Auth;
using System.Security.Claims;

namespace MiniERP.API.Services.Interfaces;

// Rozhraní definuje autentizační operace
public interface IAuthService
{
    // Metoda přihlásí uživatele a vrátí tokeny
    Task<LoginResponse> LoginAsync(LoginRequest request, string? ipAddress);

    // Metoda obnoví JWT token pomocí refresh tokenu
    Task<LoginResponse> RefreshAsync(RefreshTokenRequest request, string? ipAddress);

    // Metoda odhlásí uživatele zneplatněním refresh tokenu
    Task LogoutAsync(LogoutRequest request, string? ipAddress);

    // Metoda vrátí aktuálně přihlášeného uživatele
    Task<CurrentUserResponse?> GetCurrentUserAsync(ClaimsPrincipal user);
}