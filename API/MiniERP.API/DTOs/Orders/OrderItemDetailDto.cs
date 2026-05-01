namespace MiniERP.API.DTOs.Orders;

// Detail položky objednávky
public class OrderItemDetailDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal VatRate { get; set; }
    public decimal? DiscountPercent { get; set; }
    public decimal LineSubtotal { get; set; }
    public decimal LineVatAmount { get; set; }
    public decimal LineTotal { get; set; }
}