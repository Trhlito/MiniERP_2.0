namespace MiniERP.API.DTOs.Auth;

// DTO uchovává přihlašovací údaje uživatele
public class LoginRequest
{
    // Uživatelské jméno nebo e-mail
    public string UserNameOrEmail { get; set; } = string.Empty;

    // Heslo uživatele
    public string Password { get; set; } = string.Empty;
}