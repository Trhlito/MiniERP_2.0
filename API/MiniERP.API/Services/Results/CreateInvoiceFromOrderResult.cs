namespace MiniERP.API.Services.Results;

// Výsledek vytvoření faktury z objednávky
public class CreateInvoiceFromOrderResult
{
    // Stav úspěchu operace
    public bool Success { get; set; }

    // ID vytvořené faktury
    public int? InvoiceId { get; set; }

    // Kód chyby pro controller
    public string? ErrorCode { get; set; }

    // Text zprávy
    public string Message { get; set; } = string.Empty;
}