using Microsoft.AspNetCore.Identity;

namespace MiniERP.Data.Entities.Auth;

// Role
public class ApplicationRole : IdentityRole<int>
{
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}