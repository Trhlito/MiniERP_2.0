namespace MiniERP.API.DTOs.Users;

// DTO vrací detail uživatele
public class UserDetailDto
{
    // Id uživatele
    public int Id { get; set; }

    // Uživatelské jméno
    public string UserName { get; set; } = string.Empty;

    // E-mail uživatele
    public string Email { get; set; } = string.Empty;

    // Jméno uživatele
    public string? FirstName { get; set; }

    // Příjmení uživatele
    public string? LastName { get; set; }

    // Aktivní stav účtu
    public bool IsActive { get; set; }

    // Datum vytvoření účtu
    public DateTime CreatedAt { get; set; }

    // Datum poslední úpravy účtu
    public DateTime? UpdatedAt { get; set; }

    // Role uživatele
    public List<string> Roles { get; set; } = new();
}