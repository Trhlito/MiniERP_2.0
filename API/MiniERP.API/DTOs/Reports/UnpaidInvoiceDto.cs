namespace MiniERP.API.DTOs.Reports;

// Report neuhrazené faktury
public class UnpaidInvoiceDto
{
    public string InvoiceNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public DateTime DueDate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingAmount { get; set; }        // Zbývající částka k úhradě
    public bool IsOverdue { get; set; }                 // Příznak po splatnosti
}