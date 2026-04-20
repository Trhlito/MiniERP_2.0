namespace MiniERP.API.DTOs.Reports;

// -- Souhrnný report tržeb a faktur --
public class SalesSummaryDto
{
    // -- Celkový počet faktur --
    public int TotalInvoices { get; set; }

    // -- Počet zaplacených faktur --
    public int PaidInvoices { get; set; }

    // -- Počet nezaplacených faktur --
    public int UnpaidInvoices { get; set; }

    // -- Celková fakturovaná částka --
    public decimal TotalRevenue { get; set; }

    // -- Součet zaplacených faktur --
    public decimal PaidRevenue { get; set; }

    // -- Součet nezaplacených faktur --
    public decimal UnpaidRevenue { get; set; }

    // -- Reálně přijaté platby --
    public decimal PaymentsReceived { get; set; }
}