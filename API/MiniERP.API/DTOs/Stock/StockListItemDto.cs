namespace MiniERP.API.DTOs.Stock;

// DTO pro seznam skladových záznamů
public class StockListItemDto
{
    // ID záznamu
    public int Id { get; set; }

    // ID skladu
    public int WarehouseId { get; set; }

    // ID produktu
    public int ProductId { get; set; }

    // Aktuální množství
    public decimal Quantity { get; set; }

    // Rezervované množství
    public decimal ReservedQuantity { get; set; }
}