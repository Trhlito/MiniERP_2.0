namespace MiniERP.API.DTOs.Users;

// DTO uchovává nové heslo uživatele
public class ResetPasswordRequest
{
    // Nové heslo uživatele
    public string NewPassword { get; set; } = string.Empty;
}