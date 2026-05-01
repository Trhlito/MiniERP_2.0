namespace MiniERP.API.DTOs.Products;

// Seznam produktů
public class ProductListItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal SalePrice { get; set; }
    public string? Unit { get; set; }
    public bool IsActive { get; set; }
}