namespace MiniERP.API.DTOs.Customers;

// -- DTO pro výpis zákazníků v seznamu --
public class CustomerListItemDto
{
    // -- Interní ID zákazníka --
    public int Id { get; set; }

    // -- Zobrazované jméno zákazníka (firma nebo celé jméno osoby) --
    public string CustomerName { get; set; } = string.Empty;

    // -- Typ zákazníka: Company / Person --
    public string CustomerType { get; set; } = string.Empty;

    // -- E-mail zákazníka --
    public string? Email { get; set; }

    // -- Telefon zákazníka --
    public string? Phone { get; set; }

    // -- Informace, zda je zákazník aktivní --
    public bool IsActive { get; set; }
}