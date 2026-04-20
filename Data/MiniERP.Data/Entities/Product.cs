public class Product
{
    public int Id { get; set; }

    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public int CategoryId { get; set; }
    public int? SupplierId { get; set; }

    public decimal PurchasePrice { get; set; }
    public decimal SalePrice { get; set; }

    public decimal VatRate { get; set; }

    public string? Unit { get; set; }

    public decimal MinimumStock { get; set; }

    public bool IsService { get; set; }
    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}