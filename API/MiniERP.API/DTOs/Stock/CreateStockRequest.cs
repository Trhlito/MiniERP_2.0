namespace MiniERP.API.DTOs.Stock;

// Žádost pro vytvoření skladového záznamu
public class CreateStockRequest
{
    public int WarehouseId { get; set; }
    public int ProductId { get; set; }
    public decimal Quantity { get; set; }
    public decimal ReservedQuantity { get; set; }
}