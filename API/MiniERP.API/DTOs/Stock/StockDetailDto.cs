namespace MiniERP.API.DTOs.Stock;

// Detail skladového záznamu
public class StockDetailDto
{
    public int Id { get; set; }
    public int WarehouseId { get; set; }
    public int ProductId { get; set; }
    public decimal Quantity { get; set; }
    public decimal ReservedQuantity { get; set; }
    public DateTime LastUpdatedAt { get; set; }
}