namespace MiniERP.API.DTOs.Stock;

// Request DTO pro úpravu skladového záznamu
public class UpdateStockRequest
{
    // Aktuální množství
    public decimal Quantity { get; set; }

    // Rezervované množství
    public decimal ReservedQuantity { get; set; }
}