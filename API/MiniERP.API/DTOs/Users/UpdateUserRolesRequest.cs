namespace MiniERP.API.DTOs.Users;

// Uchovává role přiřazené uživateli
public class UpdateUserRolesRequest
{
    public List<string> Roles { get; set; } = new();
}