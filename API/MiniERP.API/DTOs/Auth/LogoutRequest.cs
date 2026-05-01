namespace MiniERP.API.DTOs.Auth;

// Uchovává refresh token pro odhlášení
public class LogoutRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}