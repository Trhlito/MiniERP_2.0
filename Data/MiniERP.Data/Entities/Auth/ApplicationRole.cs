using Microsoft.AspNetCore.Identity;

namespace MiniERP.Data.Entities.Auth;

// Role entita pro ASP.NET Identity
public class ApplicationRole : IdentityRole<int>
{
    // Popis role
    public string? Description { get; set; }

    // Aktivní stav role
    public bool IsActive { get; set; } = true;
}