namespace MiniERP.API.DTOs.Products;

// DTO pro detail produktu
public class ProductDetailDto
{
    // ID produktu
    public int Id { get; set; }

    // Kód produktu
    public string Code { get; set; } = string.Empty;

    // Název produktu
    public string Name { get; set; } = string.Empty;

    // Popis produktu
    public string? Description { get; set; }

    // ID kategorie
    public int CategoryId { get; set; }

    // ID dodavatele
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