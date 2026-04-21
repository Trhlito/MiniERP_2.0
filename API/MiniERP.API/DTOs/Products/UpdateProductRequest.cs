namespace MiniERP.API.DTOs.Products;

// Request DTO pro úpravu produktu
public class UpdateProductRequest
{
    // Kód produktu
    public string Code { get; set; } = string.Empty;

    // Název produktu
    public string Name { get; set; } = string.Empty;

    // Popis produktu
    public string? Description { get; set; }

    // Kategorie produktu
    public int CategoryId { get; set; }

    // Dodavatel produktu
    public int? SupplierId { get; set; }

    // Nákupní cena
    public decimal PurchasePrice { get; set; }

    // Prodejní cena
    public decimal SalePrice { get; set; }

    // Sazba DPH
    public decimal VatRate { get; set; }

    // Jednotka
    public string? Unit { get; set; }

    // Minimální sklad
    public decimal MinimumStock { get; set; }

    // Příznak služby
    public bool IsService { get; set; }

    // Stav aktivity produktu
    public bool IsActive { get; set; }
}