using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiniERP.API.DTOs.Users;
using MiniERP.API.Services.Interfaces;
using MiniERP.Data.Entities.Auth;

namespace MiniERP.API.Services.Implementations;

// Služba zajišťuje správu uživatelů přes ASP.NET Identity
public class UserService : IUserService
{
    // Správa uživatelů přes ASP.NET Identity
    private readonly UserManager<ApplicationUser> _userManager;

    // Správa rolí přes ASP.NET Identity
    private readonly RoleManager<ApplicationRole> _roleManager;

    // Konstruktor služby
    public UserService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    // Metoda vrátí seznam uživatelů
    public async Task<List<UserListItemDto>> GetAllAsync()
    {
        // Načtení uživatelů z Identity
        var users = await _userManager.Users
            .OrderBy(x => x.UserName)
            .ToListAsync();

        // Příprava výsledného seznamu
        var result = new List<UserListItemDto>();

        // Doplnění rolí ke každému uživateli
        foreach (var user in users)
        {
            // Načtení rolí uživatele
            var roles = await _userManager.GetRolesAsync(user);

            // Přidání uživatele do výsledku
            result.Add(new UserListItemDto
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsActive = user.IsActive,
                Roles = roles.ToList()
            });
        }

        // Vrácení seznamu uživatelů
        return result;
    }

    // Metoda vrátí detail uživatele podle id
    public async Task<UserDetailDto?> GetByIdAsync(int id)
    {
        // Vyhledání uživatele podle id
        var user = await _userManager.FindByIdAsync(id.ToString());

        // Ošetření nenalezeného uživatele
        if (user is null)
        {
            return null;
        }

        // Načtení rolí uživatele
        var roles = await _userManager.GetRolesAsync(user);

        // Vrácení detailu uživatele
        return new UserDetailDto
        {
            Id = user.Id,
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            FirstName = user.FirstName,
            LastName = user.LastName,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            Roles = roles.ToList()
        };
    }

    // Metoda vytvoří nového uživatele
    public async Task<UserDetailDto> CreateAsync(CreateUserRequest request)
    {
        // Příprava nové uživatelské entity
        var user = new ApplicationUser
        {
            UserName = request.UserName,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            IsActive = true,
            EmailConfirmed = true,
            CreatedAt = DateTime.UtcNow
        };

        // Vytvoření uživatele přes ASP.NET Identity
        var createResult = await _userManager.CreateAsync(user, request.Password);

        // Ošetření chyby při vytvoření uživatele
        if (!createResult.Succeeded)
        {
            var errors = string.Join("; ", createResult.Errors.Select(x => x.Description));
            throw new InvalidOperationException(errors);
        }

        // Přiřazení rolí, pokud byly zadány
        if (request.Roles.Any())
        {
            // Kontrola existence všech rolí
            foreach (var role in request.Roles)
            {
                // Ošetření neexistující role
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    throw new InvalidOperationException($"Role '{role}' does not exist.");
                }
            }

            // Přiřazení rolí uživateli
            var roleResult = await _userManager.AddToRolesAsync(user, request.Roles);

            // Ošetření chyby při přiřazení rolí
            if (!roleResult.Succeeded)
            {
                var errors = string.Join("; ", roleResult.Errors.Select(x => x.Description));
                throw new InvalidOperationException(errors);
            }
        }

        // Vrácení vytvořeného uživatele
        return (await GetByIdAsync(user.Id))!;
    }

    // Metoda upraví existujícího uživatele
    public async Task<bool> UpdateAsync(int id, UpdateUserRequest request)
    {
        // Vyhledání uživatele podle id
        var user = await _userManager.FindByIdAsync(id.ToString());

        // Ošetření nenalezeného uživatele
        if (user is null)
        {
            return false;
        }

        // Úprava základních údajů uživatele
        user.Email = request.Email;
        user.UserName = request.Email;
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.IsActive = request.IsActive;
        user.UpdatedAt = DateTime.UtcNow;

        // Uložení změn přes ASP.NET Identity
        var result = await _userManager.UpdateAsync(user);

        // Vrácení výsledku
        return result.Succeeded;
    }

    // Metoda upraví role uživatele
    public async Task<bool> UpdateRolesAsync(int id, UpdateUserRolesRequest request)
    {
        // Vyhledání uživatele podle id
        var user = await _userManager.FindByIdAsync(id.ToString());

        // Ošetření nenalezeného uživatele
        if (user is null)
        {
            return false;
        }

        // Kontrola existence všech rolí
        foreach (var role in request.Roles)
        {
            // Ošetření neexistující role
            if (!await _roleManager.RoleExistsAsync(role))
            {
                throw new InvalidOperationException($"Role '{role}' does not exist.");
            }
        }

        // Načtení aktuálních rolí
        var currentRoles = await _userManager.GetRolesAsync(user);

        // Odebrání aktuálních rolí
        var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

        // Ošetření chyby při odebrání rolí
        if (!removeResult.Succeeded)
        {
            return false;
        }

        // Přidání nových rolí
        var addResult = await _userManager.AddToRolesAsync(user, request.Roles);

        // Vrácení výsledku
        return addResult.Succeeded;
    }

    // Metoda nastaví nové heslo uživatele
    public async Task<bool> ResetPasswordAsync(int id, ResetPasswordRequest request)
    {
        // Vyhledání uživatele podle id
        var user = await _userManager.FindByIdAsync(id.ToString());

        // Ošetření nenalezeného uživatele
        if (user is null)
        {
            return false;
        }

        // Vygenerování reset tokenu hesla
        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

        // Nastavení nového hesla přes ASP.NET Identity
        var result = await _userManager.ResetPasswordAsync(
            user,
            resetToken,
            request.NewPassword);

        // Vrácení výsledku
        return result.Succeeded;
    }
}