namespace MiniERP.API.DTOs.Reports;

// Report obchodních výsledků podle zákazníků
public class SalesByCustomerDto
{
    // ID zákazníka
    public int CustomerId { get; set; }

    // Název zákazníka
    public string CustomerName { get; set; } = string.Empty;

    // Počet objednávek
    public int TotalOrders { get; set; }

    // Počet faktur
    public int TotalInvoices { get; set; }

    // Celková fakturovaná částka
    public decimal TotalRevenue { get; set; }

    // Celková uhrazená částka
    public decimal PaidAmount { get; set; }

    // Zbývající částka k úhradě
    public decimal RemainingAmount { get; set; }
}