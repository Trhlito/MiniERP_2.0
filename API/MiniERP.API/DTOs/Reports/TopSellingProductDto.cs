namespace MiniERP.API.DTOs.Reports;

// Report nejprodávanějších produktů
public class TopSellingProductDto
{
    public int ProductId { get; set; }
    public string ProductCode { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public decimal TotalQuantitySold { get; set; }
    public int OrderCount { get; set; }             // Počet objednávek s produktem
    public decimal TotalRevenue { get; set; }       // Celková tržba za produkt
}