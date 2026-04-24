namespace MiniERP.API.DTOs.Reports;

// Report skladového upozornění
public class StockAlertDto
{
    // Kód produktu
    public string ProductCode { get; set; } = string.Empty;

    // Název produktu
    public string ProductName { get; set; } = string.Empty;

    // Kód skladu
    public string WarehouseCode { get; set; } = string.Empty;

    // Název skladu
    public string WarehouseName { get; set; } = string.Empty;

    // Aktuální množství na skladě
    public decimal Quantity { get; set; }

    // Rezervované množství
    public decimal ReservedQuantity { get; set; }

    // Minimální skladová zásoba
    public decimal MinimumStock { get; set; }

    // Disponibilní množství
    public decimal AvailableQuantity { get; set; }

    // Stav skladu
    public string StockStatus { get; set; } = string.Empty;
}