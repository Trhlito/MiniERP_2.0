using Microsoft.AspNetCore.Identity;

namespace MiniERP.Data.Entities.Auth;

// Uživatelská entita pro ASP.NET Identity
public class ApplicationUser : IdentityUser<int>
{
    // Křestní jméno uživatele
    public string? FirstName { get; set; }

    // Příjmení uživatele
    public string? LastName { get; set; }

    // Aktivní stav účtu
    public bool IsActive { get; set; } = true;

    // Datum vytvoření účtu
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Datum poslední změny
    public DateTime? UpdatedAt { get; set; }
}