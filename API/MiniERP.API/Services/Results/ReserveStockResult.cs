namespace MiniERP.API.Services.Results;

// -- Výsledek rezervace skladu pro objednávku --
public class ReserveStockResult
{
    // -- Indikace úspěchu --
    public bool Success { get; set; }

    // -- Kód chyby pro controller --
    public string? ErrorCode { get; set; }

    // -- Textová zpráva --
    public string Message { get; set; } = string.Empty;
}