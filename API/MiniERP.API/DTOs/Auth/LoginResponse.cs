namespace MiniERP.API.DTOs.Auth;

// DTO vrací výsledek úspěšného přihlášení
public class LoginResponse
{
    // JWT token pro autentizaci
    public string Token { get; set; } = string.Empty;

    // Refresh token pro obnovení přihlášení
    public string RefreshToken { get; set; } = string.Empty;

    // Datum a čas expirace tokenu
    public DateTime ExpiresAt { get; set; }

    // Id přihlášeného uživatele
    public int UserId { get; set; }

    // Uživatelské jméno
    public string UserName { get; set; } = string.Empty;

    // E-mail uživatele
    public string Email { get; set; } = string.Empty;

    // Role uživatele
    public List<string> Roles { get; set; } = new();
}