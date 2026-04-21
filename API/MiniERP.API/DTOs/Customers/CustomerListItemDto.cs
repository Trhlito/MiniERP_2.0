namespace MiniERP.API.DTOs.Customers;

// DTO pro výpis zákazníků v seznamu
public class CustomerListItemDto
{
    // Interní ID zákazníka
    public int Id { get; set; }

    // Zobrazované jméno zákazníka
    public string CustomerName { get; set; } = string.Empty;

    // Typ zákazníka
    public string CustomerType { get; set; } = string.Empty;

    // E-mail zákazníka
    public string? Email { get; set; }

    // Telefon zákazníka
    public string? Phone { get; set; }

    // Stav aktivity zákazníka
    public bool IsActive { get; set; }
}