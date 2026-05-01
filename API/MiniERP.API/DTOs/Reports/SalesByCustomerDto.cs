namespace MiniERP.API.DTOs.Reports;

// Report obchodních výsledků podle zákazníků
public class SalesByCustomerDto
{
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public int TotalOrders { get; set; }
    public int TotalInvoices { get; set; }
    public decimal TotalRevenue { get; set; }       // Celková fakturovaná částka
    public decimal PaidAmount { get; set; }         // Celková uhrazená částka
    public decimal RemainingAmount { get; set; }    // Zbývající částka k úhradě
}