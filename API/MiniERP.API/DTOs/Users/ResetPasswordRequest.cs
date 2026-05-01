namespace MiniERP.API.DTOs.Users;

// Uchovává nové heslo uživatele
public class ResetPasswordRequest
{
    public string NewPassword { get; set; } = string.Empty;         // Nové heslo uživatele
}