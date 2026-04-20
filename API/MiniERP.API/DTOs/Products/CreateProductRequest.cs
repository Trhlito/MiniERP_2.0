namespace MiniERP.API.DTOs.Products;

// -- Request DTO pro vytvoření nového produktu --
public class CreateProductRequest
{
    // -- Kód produktu --
    public string Code { get; set; } = string.Empty;

    // -- Název produktu --
    public string Name { get; set; } = string.Empty;

    // -- Popis produktu --
    public string? Description { get; set; }

    // -- Kategorie produktu --
    public int CategoryId { get; set; }

    // -- Dodavatel produktu --
    public int? SupplierId { get; set; }

    // -- Nákupní cena --
    public decimal PurchasePrice { get; set; }

    // -- Prodejní cena --
    public decimal SalePrice { get; set; }

    // -- Sazba DPH --
    public decimal VatRate { get; set; }

    // -- Jednotka --
    public string? Unit { get; set; }

    // -- Minimální sklad --
    public decimal MinimumStock { get; set; }

    // -- Je produkt služba --
    public bool IsService { get; set; }

    // -- Aktivní produkt --
    public bool IsActive { get; set; } = true;
}