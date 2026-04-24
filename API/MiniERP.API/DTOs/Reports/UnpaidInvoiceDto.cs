namespace MiniERP.API.DTOs.Reports;

// Report neuhrazené faktury
public class UnpaidInvoiceDto
{
    // Číslo faktury
    public string InvoiceNumber { get; set; } = string.Empty;

    // Název zákazníka
    public string CustomerName { get; set; } = string.Empty;

    // Datum vystavení
    public DateTime IssueDate { get; set; }

    // Datum splatnosti
    public DateTime DueDate { get; set; }

    // Celková částka faktury
    public decimal TotalAmount { get; set; }

    // Uhrazena částka
    public decimal PaidAmount { get; set; }

    // Zbývající částka k úhradě
    public decimal RemainingAmount { get; set; }

    // Příznak po splatnosti
    public bool IsOverdue { get; set; }
}