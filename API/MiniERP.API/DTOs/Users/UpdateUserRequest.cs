namespace MiniERP.API.DTOs.Users;

// Uchovává data pro úpravu uživatele
public class UpdateUserRequest
{
    public string Email { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public bool IsActive { get; set; }
}