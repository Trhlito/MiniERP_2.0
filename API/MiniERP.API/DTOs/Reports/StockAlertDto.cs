namespace MiniERP.API.DTOs.Reports;

// Report skladového upozornění
public class StockAlertDto
{
    public string ProductCode { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string WarehouseCode { get; set; } = string.Empty;
    public string WarehouseName { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal ReservedQuantity { get; set; }
    public decimal MinimumStock { get; set; }
    public decimal AvailableQuantity { get; set; }          // Disponibilní množství

    public string StockStatus { get; set; } = string.Empty;
}