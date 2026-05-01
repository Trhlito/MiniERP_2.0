namespace MiniERP.API.DTOs.Stock;

// Seznam skladových záznamů
public class StockListItemDto
{
    public int Id { get; set; }
    public int WarehouseId { get; set; }
    public int ProductId { get; set; }
    public decimal Quantity { get; set; }
    public decimal ReservedQuantity { get; set; }
}