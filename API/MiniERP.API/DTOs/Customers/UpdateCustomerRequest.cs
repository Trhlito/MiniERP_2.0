namespace MiniERP.API.DTOs.Customers;

// DTO pro úpravu existujícího zákazníka
public class UpdateCustomerRequest
{
    // Typ zákazníka
    public string CustomerType { get; set; } = string.Empty;

    // Název firmy
    public string? CompanyName { get; set; }

    // Jméno osoby
    public string? FirstName { get; set; }

    // Příjmení osoby
    public string? LastName { get; set; }

    // E-mail zákazníka
    public string? Email { get; set; }

    // Telefon zákazníka
    public string? Phone { get; set; }

    // Ulice
    public string? Street { get; set; }

    // Město
    public string? City { get; set; }

    // PSČ
    public string? ZipCode { get; set; }

    // Země
    public string? Country { get; set; }

    // IČO firmy
    public string? ICO { get; set; }

    // DIČ firmy
    public string? DIC { get; set; }

    // Stav aktivity zákazníka
    public bool IsActive { get; set; }
}