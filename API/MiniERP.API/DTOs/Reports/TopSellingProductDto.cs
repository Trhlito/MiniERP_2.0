namespace MiniERP.API.DTOs.Reports;

// Report nejprodávanějších produktů
public class TopSellingProductDto
{
    // ID produktu
    public int ProductId { get; set; }

    // Kód produktu
    public string ProductCode { get; set; } = string.Empty;

    // Název produktu
    public string ProductName { get; set; } = string.Empty;

    // Celkové prodané množství
    public decimal TotalQuantitySold { get; set; }

    // Počet objednávek s produktem
    public int OrderCount { get; set; }

    // Celková tržba za produkt
    public decimal TotalRevenue { get; set; }
}