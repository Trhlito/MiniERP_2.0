namespace MiniERP.API.DTOs.Reports;

// Souhrnný report obchodních dat za zadané období
public class SalesSummaryDto
{
    public int TotalOrders { get; set; }
    public int ConfirmedOrders { get; set; }
    public int TotalInvoices { get; set; }
    public int PaidInvoices { get; set; }
    public decimal TotalInvoiceAmount { get; set; }
    public decimal TotalPaidAmount { get; set; }    // Celková hodnota přijatých plateb
    public decimal RemainingAmount { get; set; }
}