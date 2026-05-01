namespace MiniERP.API.DTOs.Stock;

// Žádost pro úpravu skladového záznamu
public class UpdateStockRequest
{
    public decimal Quantity { get; set; }
    public decimal ReservedQuantity { get; set; }
}