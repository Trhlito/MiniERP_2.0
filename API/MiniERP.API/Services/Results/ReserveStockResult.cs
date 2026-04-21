namespace MiniERP.API.Services.Results;

// Výsledek rezervace skladu pro objednávku
public class ReserveStockResult
{
    // Stav úspěchu operace
    public bool Success { get; set; }

    // Kód chyby pro controller
    public string? ErrorCode { get; set; }

    // Text zprávy
    public string Message { get; set; } = string.Empty;
}