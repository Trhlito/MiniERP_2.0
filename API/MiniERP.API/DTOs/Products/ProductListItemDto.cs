namespace MiniERP.API.DTOs.Products;

// -- DTO pro seznam produktů (zjednodušený pohled pro API) --
public class ProductListItemDto
{
    public int Id { get; set; }

    // -- Název produktu --
    public string Name { get; set; } = string.Empty;

    // -- Prodejní cena (SalePrice z DB) --
    public decimal SalePrice { get; set; }

    // -- Jednotka (ks, kg, m...) --
    public string? Unit { get; set; }

    // -- Aktivní produkt --
    public bool IsActive { get; set; }
}