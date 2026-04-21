namespace MiniERP.API.DTOs.Invoices;

// DTO pro detail položky faktury
public class InvoiceItemDetailDto
{
    // ID položky
    public int Id { get; set; }

    // ID produktu
    public int? ProductId { get; set; }

    // Název položky
    public string ItemName { get; set; } = string.Empty;

    // Množství
    public decimal Quantity { get; set; }

    // Cena za jednotku
    public decimal UnitPrice { get; set; }

    // Sazba DPH
    public decimal VatRate { get; set; }

    // Sleva v procentech
    public decimal? DiscountPercent { get; set; }

    // Mezisoučet řádku
    public decimal LineSubtotal { get; set; }

    // DPH řádku
    public decimal LineVatAmount { get; set; }

    // Celková cena řádku
    public decimal LineTotal { get; set; }
}