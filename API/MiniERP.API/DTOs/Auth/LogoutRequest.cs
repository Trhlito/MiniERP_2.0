namespace MiniERP.API.DTOs.Auth;

// DTO uchovává refresh token pro odhlášení
public class LogoutRequest
{
    // Refresh token, který má být zneplatněn
    public string RefreshToken { get; set; } = string.Empty;
}