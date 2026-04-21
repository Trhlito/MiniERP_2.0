namespace MiniERP.Data.Entities;

// Entita zákazníka
public class Customer
{
    // Interní ID zákazníka
    public int Id { get; set; }

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
    public bool IsActive { get; set; } = true;

    // Datum vytvoření záznamu
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Datum poslední úpravy záznamu
    public DateTime? UpdatedAt { get; set; }

    // Složené jméno zákazníka
    public string FullName =>
    CustomerType == "Company"
        ? CompanyName ?? string.Empty
        : $"{FirstName} {LastName}".Trim();
}