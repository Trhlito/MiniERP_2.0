using Microsoft.AspNetCore.Identity;
using MiniERP.Data.Entities.Auth;

namespace MiniERP.API.Seed;

// Seeder
public static class IdentitySeeder
{
    // Vytvoříme základní role a výchozí admin účet
    public static async Task SeedAsync(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        // Načtení služeb pro správu rolí a uživatelů
        var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // Základní role
        var roles = new[]
        {
            new ApplicationRole { Name = "Admin", NormalizedName = "ADMIN", Description = "Plný přístup k systému" },
            new ApplicationRole { Name = "Manager", NormalizedName = "MANAGER", Description = "Manažerská role" },
            new ApplicationRole { Name = "Employee", NormalizedName = "EMPLOYEE", Description = "Zaměstnanec" },
            new ApplicationRole { Name = "User", NormalizedName = "USER", Description = "Základní uživatel" }
        };

        // Vytvoření rolí, pokud ještě neexistují
        foreach (var role in roles)
        {
            var exists = await roleManager.RoleExistsAsync(role.Name!);
            if (exists)
            {
                continue;
            }
            await roleManager.CreateAsync(role);
        }

        // Načtení a kontrola hodnot admin účtu z konfigurace
        var adminEmail = configuration["SeedAdmin:Email"];
        var adminUserName = configuration["SeedAdmin:UserName"];
        var adminPassword = configuration["SeedAdmin:Password"];
        var adminFirstName = configuration["SeedAdmin:FirstName"];
        var adminLastName = configuration["SeedAdmin:LastName"];

        if (string.IsNullOrWhiteSpace(adminEmail) ||
            string.IsNullOrWhiteSpace(adminUserName) ||
            string.IsNullOrWhiteSpace(adminPassword))
        {
            throw new InvalidOperationException("SeedAdmin configuration is missing required values.");
        }

        // Vyhledání admin účtu dle e-mailu, vytvoření přípravy admin pokud null
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser is null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminUserName,
                Email = adminEmail,
                FirstName = adminFirstName,
                LastName = adminLastName,
                EmailConfirmed = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            // Vytvoření uživatele přes ASP + ošetření chyby při vytvoření
            var createResult = await userManager.CreateAsync(adminUser, adminPassword);

            if (!createResult.Succeeded)
            {
                var errors = string.Join("; ", createResult.Errors.Select(x => x.Description));
                throw new InvalidOperationException($"Admin user seed failed: {errors}");
            }
        }

        // Kontrola, a přiřazení role Admin
        var isAdmin = await userManager.IsInRoleAsync(adminUser, "Admin");

        if (!isAdmin)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}