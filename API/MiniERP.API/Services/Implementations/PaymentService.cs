using Microsoft.EntityFrameworkCore;
using MiniERP.API.DTOs.Payments;
using MiniERP.API.Services.Interfaces;
using MiniERP.Data;
using MiniERP.Data.Entities;

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

    // Vytvoření nové platby
    public async Task<int> CreateAsync(CreatePaymentRequest request)
    {
        // Načtení faktury
        var invoice = await _db.Invoices
            .FirstOrDefaultAsync(i => i.Id == request.InvoiceId);

        if (invoice == null)
        {
            throw new Exception("Faktura neexistuje.");
        }

        // Součet již zaplacených plateb
        var alreadyPaid = await _db.Payments
            .Where(p => p.InvoiceId == request.InvoiceId)
            .SumAsync(p => (decimal?)p.Amount) ?? 0m;

        // Výpočet zbývající částky
        var remaining = invoice.TotalAmount - alreadyPaid;

        // Blokace přeplatku
        if (request.Amount > remaining)
        {
            throw new Exception(
                $"Platba je vyšší než zbývající částka. Zbývá uhradit {remaining}.");
        }

        // Vytvoření platby
        var payment = new Payment
        {
            InvoiceId = request.InvoiceId,
            PaymentDate = request.PaymentDate,
            Amount = request.Amount,
            PaymentMethod = request.PaymentMethod,
            ReferenceNumber = request.ReferenceNumber,
            Note = request.Note,
            CreatedByUserId = request.CreatedByUserId,
            CreatedAt = DateTime.UtcNow
        };

        _db.Payments.Add(payment);
        await _db.SaveChangesAsync();

        // Přepočet po nové platbě
        var newPaidTotal = alreadyPaid + request.Amount;

        // Nastavení stavu Paid při plné úhradě
        if (newPaidTotal >= invoice.TotalAmount)
        {
            invoice.Status = "Paid";
            invoice.PaidDate = DateTime.UtcNow;
        }
        else
        {
            // Ponechání stavu Issued při částečné úhradě
            invoice.Status = "Issued";
            invoice.PaidDate = null;
        }

        invoice.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return payment.Id;
    }
}