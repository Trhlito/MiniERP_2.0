namespace MiniERP.API.DTOs.StockMovements;

// Seznam pohybů skladu
public class StockMovementListItemDto
{
    public int Id { get; set; }
    public int WarehouseId { get; set; }
    public int ProductId { get; set; }
    public string MovementType { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public string? ReferenceType { get; set; }
    public int? ReferenceId { get; set; }
    public DateTime CreatedAt { get; set; }
}