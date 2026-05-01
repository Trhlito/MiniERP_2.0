namespace MiniERP.API.DTOs.Auth;

// Uchovává přihlašovací údaje uživatele
public class LoginRequest
{
    public string UserNameOrEmail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
} 