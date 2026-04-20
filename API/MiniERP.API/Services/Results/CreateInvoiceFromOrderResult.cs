namespace MiniERP.API.Services.Results;

// -- Výsledek vytvoření faktury z objednávky --
public class CreateInvoiceFromOrderResult
{
    // -- Indikace úspěchu operace --
    public bool Success { get; set; }

    // -- ID nově vytvořené faktury --
    public int? InvoiceId { get; set; }

    // -- Kód chyby pro controller --
    public string? ErrorCode { get; set; }

    // -- Textová zpráva --
    public string Message { get; set; } = string.Empty;
}