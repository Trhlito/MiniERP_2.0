namespace MiniERP.API.DTOs.Reports;

// Souhrnný report obchodních dat za zadané období
public class SalesSummaryDto
{
    // Celkový počet objednávek
    public int TotalOrders { get; set; }

    // Počet potvrzených objednávek
    public int ConfirmedOrders { get; set; }

    // Celkový počet faktur
    public int TotalInvoices { get; set; }

    // Počet uhrazených faktur
    public int PaidInvoices { get; set; }

    // Celková hodnota faktur
    public decimal TotalInvoiceAmount { get; set; }

    // Celková hodnota přijatých plateb
    public decimal TotalPaidAmount { get; set; }

    // Zbývající částka k úhradě
    public decimal RemainingAmount { get; set; }
}