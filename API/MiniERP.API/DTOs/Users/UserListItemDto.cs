namespace MiniERP.API.DTOs.Users;

// Vrací základní informace o uživateli v seznamu
public class UserListItemDto
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public bool IsActive { get; set; }
    public List<string> Roles { get; set; } = new();
}