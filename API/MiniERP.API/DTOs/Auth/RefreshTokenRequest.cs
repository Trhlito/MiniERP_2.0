namespace MiniERP.API.DTOs.Auth;

// Uchovává refresh token pro obnovení přihlášení
public class RefreshTokenRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}