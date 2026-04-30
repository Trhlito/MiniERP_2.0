namespace MiniERP.API.DTOs.Users;

// DTO uchovává role přiřazené uživateli
public class UpdateUserRolesRequest
{
    // Role uživatele
    public List<string> Roles { get; set; } = new();
}