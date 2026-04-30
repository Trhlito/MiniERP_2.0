namespace MiniERP.API.DTOs.Auth;

// DTO vrací aktuálně přihlášeného uživatele
public class CurrentUserResponse
{
    // Id aktuálně přihlášeného uživatele
    public int UserId { get; set; }

    // Uživatelské jméno
    public string UserName { get; set; } = string.Empty;

    // E-mail uživatele
    public string Email { get; set; } = string.Empty;

    // Jméno uživatele
    public string? FirstName { get; set; }

    // Příjmení uživatele
    public string? LastName { get; set; }

    // Role uživatele
    public List<string> Roles { get; set; } = new();
}