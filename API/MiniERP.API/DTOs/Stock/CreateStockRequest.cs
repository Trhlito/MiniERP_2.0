namespace MiniERP.API.DTOs.Stock;

// -- Request DTO pro vytvoření skladového záznamu --
public class CreateStockRequest
{
    // -- ID skladu --
    public int WarehouseId { get; set; }

    // -- ID produktu --
    public int ProductId { get; set; }

    // -- Aktuální množství --
    public decimal Quantity { get; set; }

    // -- Rezervované množství --
    public decimal ReservedQuantity { get; set; }
}