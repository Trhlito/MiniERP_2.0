namespace MiniERP.API.DTOs.Customers;

// Výpis zákazníků v seznamu
public class CustomerListItemDto
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerType { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public bool IsActive { get; set; }
}