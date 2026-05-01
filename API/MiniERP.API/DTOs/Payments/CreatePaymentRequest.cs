namespace MiniERP.API.DTOs.Payments;

// Žádost pro vytvoření platby
public class CreatePaymentRequest
{
    public int InvoiceId { get; set; }
    public DateTime PaymentDate { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string? ReferenceNumber { get; set; }
    public string? Note { get; set; }
    public int CreatedByUserId { get; set; }
}