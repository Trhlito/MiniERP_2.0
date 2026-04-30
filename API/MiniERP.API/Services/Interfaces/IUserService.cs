using MiniERP.API.DTOs.Users;

namespace MiniERP.API.Services.Interfaces;

// Rozhraní definuje operace pro správu uživatelů
public interface IUserService
{
    // Metoda vrátí seznam uživatelů
    Task<List<UserListItemDto>> GetAllAsync();

    // Metoda vrátí detail uživatele podle id
    Task<UserDetailDto?> GetByIdAsync(int id);

    // Metoda vytvoří nového uživatele
    Task<UserDetailDto> CreateAsync(CreateUserRequest request);

    // Metoda upraví existujícího uživatele
    Task<bool> UpdateAsync(int id, UpdateUserRequest request);

    // Metoda upraví role uživatele
    Task<bool> UpdateRolesAsync(int id, UpdateUserRolesRequest request);

    // Metoda nastaví nové heslo uživatele
    Task<bool> ResetPasswordAsync(int id, ResetPasswordRequest request);
}