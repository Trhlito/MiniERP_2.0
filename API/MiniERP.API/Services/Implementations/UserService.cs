using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiniERP.API.DTOs.Users;
using MiniERP.API.Services.Interfaces;
using MiniERP.Data.Entities.Auth;

namespace MiniERP.API.Services.Implementations;

public class UserService : IUserService
{
    // Správa uživatelů a rolí přes ASP.NET Identity
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    // Konstruktor služby
    public UserService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    // Seznam uživatelů
    public async Task<List<UserListItemDto>> GetAllAsync()
    {
        // Načtu uživatele z Identity
        var users = await _userManager.Users
            .OrderBy(x => x.UserName)
            .ToListAsync();

        // Příprava výsledného seznamu
        var result = new List<UserListItemDto>();

        // Doplnění rolí ke každému uživateli
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);

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
        return result;
    }

    // Detail uživatele podle id
    public async Task<UserDetailDto?> GetByIdAsync(int id)
    {
        // Vyhledání dle id + Ošetření null - uživatele
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user is null)
        {
            return null;
        }

        // Načteme role
        var roles = await _userManager.GetRolesAsync(user);
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

    // Vytvoření nového uživatele a příprava nové už. entity
    public async Task<UserDetailDto> CreateAsync(CreateUserRequest request)
    {
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


        // Vytvoření uživatele přes ASP.NET Identity + ošwtření chyby
        var createResult = await _userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
        {
            var errors = string.Join("; ", createResult.Errors.Select(x => x.Description));
            throw new InvalidOperationException(errors);
        }


        // Přiřazení rolí, pokud byly zadány + kontrola zda existují všechny role
        if (request.Roles.Any())
        {
            foreach (var role in request.Roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    throw new InvalidOperationException($"Role '{role}' does not exist.");
                }
            }


            // Přiřazení rolí uživateli + ošetření chyby při přiřazení
            var roleResult = await _userManager.AddToRolesAsync(user, request.Roles);
            if (!roleResult.Succeeded)
            {
                var errors = string.Join("; ", roleResult.Errors.Select(x => x.Description));
                throw new InvalidOperationException(errors);
            }
        }
        return (await GetByIdAsync(user.Id))!;
    }

    // Upravení existujícího uživatele
    public async Task<bool> UpdateAsync(int id, UpdateUserRequest request)
    {
        // Vyhledání uživatele podle id + ošetření null zase
        var user = await _userManager.FindByIdAsync(id.ToString());
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

        return result.Succeeded;
    }

    // Úprava role uživatele
    public async Task<bool> UpdateRolesAsync(int id, UpdateUserRolesRequest request)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user is null)
        {
            return false;
        }


        foreach (var role in request.Roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                throw new InvalidOperationException($"Role '{role}' does not exist.");
            }
        }

        // Načtení a odebrání aktuálních rolí , ošetření chyby
        var currentRoles = await _userManager.GetRolesAsync(user);
        var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
        if (!removeResult.Succeeded)
        {
            return false;
        }


        // Přidání nových rolí
        var addResult = await _userManager.AddToRolesAsync(user, request.Roles);
        return addResult.Succeeded;
    }

    // Metoda nastaví nové heslo uživatele
    public async Task<bool> ResetPasswordAsync(int id, ResetPasswordRequest request)
    {
        // Vyhledání + ošetření
        var user = await _userManager.FindByIdAsync(id.ToString());
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

        return result.Succeeded;
    }
}