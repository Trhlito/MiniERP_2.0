namespace MiniERP.API.DTOs.Users;

// DTO vrací základní informace o uživateli v seznamu
public class UserListItemDto
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

    // Role uživatele
    public List<string> Roles { get; set; } = new();
}