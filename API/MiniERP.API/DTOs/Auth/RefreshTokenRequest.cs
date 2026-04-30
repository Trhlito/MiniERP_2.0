namespace MiniERP.API.DTOs.Auth;

// DTO uchovává refresh token pro obnovení přihlášení
public class RefreshTokenRequest
{
    // Refresh token uživatele
    public string RefreshToken { get; set; } = string.Empty;
}