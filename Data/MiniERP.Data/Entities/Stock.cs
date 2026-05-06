namespace MiniERP.Data.Entities;

// Sklad
public class Stock
{
    public int Id { get; set; }
    public int WarehouseId { get; set; }
    public int ProductId { get; set; }
    public decimal Quantity { get; set; }
    public decimal ReservedQuantity { get; set; }
    public DateTime LastUpdatedAt { get; set; }
}