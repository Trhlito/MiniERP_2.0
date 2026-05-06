namespace MiniERP.Data.Entities;

// Pohyb skladu
public class StockMovement
{
    public int Id { get; set; }
    public int StockId { get; set; }
    public int WarehouseId { get; set; }
    public int ProductId { get; set; }
    public string MovementType { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal QuantityBefore { get; set; }
    public decimal QuantityAfter { get; set; }
    public decimal ReservedBefore { get; set; }
    public decimal ReservedAfter { get; set; }
    public string? ReferenceType { get; set; }
    public int? ReferenceId { get; set; }
    public string? Note { get; set; }
    public int CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; }
}