namespace MiniERP.API.DTOs.Users;

// DTO uchovává data pro úpravu uživatele
public class UpdateUserRequest
{
    // E-mail uživatele
    public string Email { get; set; } = string.Empty;

    // Jméno uživatele
    public string? FirstName { get; set; }

    // Příjmení uživatele
    public string? LastName { get; set; }

    // Aktivní stav účtu
    public bool IsActive { get; set; }
}