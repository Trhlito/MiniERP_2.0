using Microsoft.AspNetCore.Identity;

namespace MiniERP.Data.Entities.Auth;

// Uživatelská entita pro ASP.NET 
public class ApplicationUser : IdentityUser<int>
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}