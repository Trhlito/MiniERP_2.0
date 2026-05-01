using Microsoft.AspNetCore.Identity;
using MiniERP.Data.Entities.Auth;

namespace MiniERP.API.Seed;

// Seeder připraví základní role a výchozí admin účet pro první spuštění.
public static class IdentitySeeder
{
    // Metoda vytvoří základní role a výchozí admin účet
    public static async Task SeedAsync(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        // Načtení služby pro správu rolí
        var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        // Načtení služby pro správu uživatelů
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // Seznam základních rolí systému
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
            // Kontrola existence role podle názvu
            var exists = await roleManager.RoleExistsAsync(role.Name!);

            // Přeskočení již existující role
            if (exists)
            {
                continue;
            }

            // Vytvoření nové role
            await roleManager.CreateAsync(role);
        }

        // Načtení hodnot výchozího admin účtu z konfigurace
        var adminEmail = configuration["SeedAdmin:Email"];
        var adminUserName = configuration["SeedAdmin:UserName"];
        var adminPassword = configuration["SeedAdmin:Password"];
        var adminFirstName = configuration["SeedAdmin:FirstName"];
        var adminLastName = configuration["SeedAdmin:LastName"];

        // Kontrola povinných hodnot pro admin účet
        if (string.IsNullOrWhiteSpace(adminEmail) ||
            string.IsNullOrWhiteSpace(adminUserName) ||
            string.IsNullOrWhiteSpace(adminPassword))
        {
            throw new InvalidOperationException("SeedAdmin configuration is missing required values.");
        }

        // Vyhledání admin účtu podle e-mailu
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        // Vytvoření admin účtu, pokud ještě neexistuje
        if (adminUser is null)
        {
            // Příprava výchozího admin uživatele
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

            // Vytvoření uživatele přes ASP.NET Identity
            var createResult = await userManager.CreateAsync(adminUser, adminPassword);

            // Ošetření chyby při vytvoření uživatele
            if (!createResult.Succeeded)
            {
                var errors = string.Join("; ", createResult.Errors.Select(x => x.Description));
                throw new InvalidOperationException($"Admin user seed failed: {errors}");
            }
        }

        // Kontrola, zda má admin přiřazenou roli Admin
        var isAdmin = await userManager.IsInRoleAsync(adminUser, "Admin");

        // Přiřazení role Admin, pokud chybí
        if (!isAdmin)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}