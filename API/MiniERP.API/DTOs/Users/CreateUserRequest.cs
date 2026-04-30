namespace MiniERP.API.DTOs.Users;

// DTO uchovává data pro vytvoření uživatele
public class CreateUserRequest
{
    // Uživatelské jméno
    public string UserName { get; set; } = string.Empty;

    // E-mail uživatele
    public string Email { get; set; } = string.Empty;

    // Heslo uživatele
    public string Password { get; set; } = string.Empty;

    // Jméno uživatele
    public string? FirstName { get; set; }

    // Příjmení uživatele
    public string? LastName { get; set; }

    // Role uživatele
    public List<string> Roles { get; set; } = new();
}