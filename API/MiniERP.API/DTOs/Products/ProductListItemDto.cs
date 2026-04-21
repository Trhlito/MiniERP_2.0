namespace MiniERP.API.DTOs.Products;

// DTO pro seznam produktů
public class ProductListItemDto
{
    // ID produktu
    public int Id { get; set; }

    // Název produktu
    public string Name { get; set; } = string.Empty;

    // Prodejní cena
    public decimal SalePrice { get; set; }

    // Jednotka
    public string? Unit { get; set; }

    // Stav aktivity produktu
    public bool IsActive { get; set; }
}