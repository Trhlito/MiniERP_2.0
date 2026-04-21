public class Product
{
    // Primární klíč
    public int Id { get; set; }

    // Kód produktu
    public string Code { get; set; } = string.Empty;

    // Název produktu
    public string Name { get; set; } = string.Empty;

    // Popis produktu
    public string? Description { get; set; }

    // Odkaz na kategorii
    public int CategoryId { get; set; }

    // Odkaz na dodavatele
    public int? SupplierId { get; set; }

    // Nákupní cena
    public decimal PurchasePrice { get; set; }

    // Prodejní cena
    public decimal SalePrice { get; set; }

    // Sazba DPH
    public decimal VatRate { get; set; }

    // Jednotka
    public string? Unit { get; set; }

    // Minimální skladové množství
    public decimal MinimumStock { get; set; }

    // Příznak služby
    public bool IsService { get; set; }

    // Stav aktivity produktu
    public bool IsActive { get; set; }

    // Datum vytvoření
    public DateTime CreatedAt { get; set; }

    // Datum poslední úpravy
    public DateTime? UpdatedAt { get; set; }
}