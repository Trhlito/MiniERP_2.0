using System.Data;
using Microsoft.EntityFrameworkCore;
using MiniERP.API.DTOs.Payments;
using MiniERP.API.Services.Interfaces;
using MiniERP.Data;


namespace MiniERP.API.Services.Implementations;

// Implementace služby pro platby
public class PaymentService : IPaymentService
{
    // Databázový kontext
    private readonly ApplicationDbContext _db;

    public PaymentService(ApplicationDbContext db)
    {
        _db = db;
    }

    // Načtení seznamu plateb
    public async Task<List<PaymentListItemDto>> GetAllAsync()
    {
        return await _db.Payments
            .AsNoTracking()
            .OrderByDescending(p => p.PaymentDate)
            .Select(p => new PaymentListItemDto
            {
                Id = p.Id,
                InvoiceId = p.InvoiceId,
                PaymentDate = p.PaymentDate,
                Amount = p.Amount,
                PaymentMethod = p.PaymentMethod,
                ReferenceNumber = p.ReferenceNumber
            })
            .ToListAsync();
    }

    // Načtení detailu platby
    public async Task<PaymentDetailDto?> GetByIdAsync(int id)
    {
        return await _db.Payments
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new PaymentDetailDto
            {
                Id = p.Id,
                InvoiceId = p.InvoiceId,
                PaymentDate = p.PaymentDate,
                Amount = p.Amount,
                PaymentMethod = p.PaymentMethod,
                ReferenceNumber = p.ReferenceNumber,
                Note = p.Note,
                CreatedByUserId = p.CreatedByUserId,
                CreatedAt = p.CreatedAt
            })
            .FirstOrDefaultAsync();
    }

    // Vytvoření nové platby //
    public async Task<int> CreateAsync(CreatePaymentRequest request)
    {
        // Načtení faktury //
        var invoice = await _db.Invoices
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == request.InvoiceId);

        if (invoice == null)
        {
            throw new Exception("Faktura neexistuje.");
        }

        // Databázové připojení z EF Core kontextu //
        var connection = _db.Database.GetDbConnection();

        // Otevření připojení při zavřeném stavu //
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
        }

        // Vytvoření databázového příkazu pro stored procedure //
        await using var command = connection.CreateCommand();
        command.CommandText = "dbo.sp_RegisterPaymentForInvoice";
        command.CommandType = CommandType.StoredProcedure;

        // Parametr ID faktury //
        var invoiceIdParameter = command.CreateParameter();
        invoiceIdParameter.ParameterName = "@InvoiceId";
        invoiceIdParameter.Value = request.InvoiceId;
        command.Parameters.Add(invoiceIdParameter);

        // Parametr částky //
        var amountParameter = command.CreateParameter();
        amountParameter.ParameterName = "@Amount";
        amountParameter.Value = request.Amount;
        command.Parameters.Add(amountParameter);

        // Parametr data platby //
        var paymentDateParameter = command.CreateParameter();
        paymentDateParameter.ParameterName = "@PaymentDate";
        paymentDateParameter.Value = request.PaymentDate;
        command.Parameters.Add(paymentDateParameter);

        // Parametr metody platby //
        var paymentMethodParameter = command.CreateParameter();
        paymentMethodParameter.ParameterName = "@PaymentMethod";
        paymentMethodParameter.Value = request.PaymentMethod;
        command.Parameters.Add(paymentMethodParameter);

        // Parametr referenčního čísla //
        var referenceNumberParameter = command.CreateParameter();
        referenceNumberParameter.ParameterName = "@ReferenceNumber";
        referenceNumberParameter.Value = string.IsNullOrWhiteSpace(request.ReferenceNumber)
            ? DBNull.Value
            : request.ReferenceNumber;
        command.Parameters.Add(referenceNumberParameter);

        // Parametr poznámky //
            var noteParameter = command.CreateParameter();
        noteParameter.ParameterName = "@Note";
        noteParameter.Value = string.IsNullOrWhiteSpace(request.Note)
            ? DBNull.Value
            : request.Note;
        command.Parameters.Add(noteParameter);

        // Parametr ID uživatele //
        var createdByUserIdParameter = command.CreateParameter();
        createdByUserIdParameter.ParameterName = "@CreatedByUserId";
        createdByUserIdParameter.Value = request.CreatedByUserId;
        command.Parameters.Add(createdByUserIdParameter);

        // Spuštění procedury a načtení výsledku //
        await using var reader = await command.ExecuteReaderAsync();

        // Kontrola vráceného výsledku //
        if (!await reader.ReadAsync())
        {
            throw new Exception("Procedura nevrátila výsledek vytvořené platby.");
        }

        // Načtení ID platby z výsledku procedury //
        var paymentIdOrdinal = reader.GetOrdinal("PaymentId");
        var paymentId = reader.GetInt32(paymentIdOrdinal);

        return paymentId;
    }
}