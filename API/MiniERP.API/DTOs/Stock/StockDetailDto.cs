namespace MiniERP.API.DTOs.Stock;

// DTO pro detail skladového záznamu
public class StockDetailDto
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

    // Datum poslední aktualizace
    public DateTime LastUpdatedAt { get; set; }
}