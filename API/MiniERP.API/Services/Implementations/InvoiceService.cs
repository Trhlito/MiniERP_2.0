using Microsoft.EntityFrameworkCore;
using MiniERP.API.DTOs.Invoices;
using MiniERP.API.Services.Interfaces;
using MiniERP.Data;
using MiniERP.Data.Entities;
using MiniERP.API.Services.Results;

namespace MiniERP.API.Services.Implementations;

// -- Implementace služby pro faktury --
public class InvoiceService : IInvoiceService
{
    // -- Databázový kontext --
    private readonly ApplicationDbContext _db;

    public InvoiceService(ApplicationDbContext db)
    {
        _db = db;
    }

    // -- Vrátí seznam faktur --
    public async Task<List<InvoiceListItemDto>> GetAllAsync()
    {
        return await _db.Invoices
            .AsNoTracking()
            .OrderByDescending(i => i.IssueDate)
            .Select(i => new InvoiceListItemDto
            {
                Id = i.Id,
                InvoiceNumber = i.InvoiceNumber,
                OrderId = i.OrderId,
                CustomerId = i.CustomerId,
                IssueDate = i.IssueDate,
                Status = i.Status,
                TotalAmount = i.TotalAmount,
                Currency = i.Currency
            })
            .ToListAsync();
    }

    // -- Vrátí detail faktury podle ID --
    public async Task<InvoiceDetailDto?> GetByIdAsync(int id)
    {
        return await _db.Invoices
            .AsNoTracking()
            .Where(i => i.Id == id)
            .Select(i => new InvoiceDetailDto
            {
                Id = i.Id,
                InvoiceNumber = i.InvoiceNumber,
                OrderId = i.OrderId,
                CustomerId = i.CustomerId,
                IssueDate = i.IssueDate,
                DueDate = i.DueDate,
                PaidDate = i.PaidDate,
                Status = i.Status,
                Subtotal = i.Subtotal,
                VatTotal = i.VatTotal,
                TotalAmount = i.TotalAmount,
                Currency = i.Currency,
                Note = i.Note,
                CreatedByUserId = i.CreatedByUserId,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt,

                // -- Položky faktury --
                Items = _db.InvoiceItems
                    .Where(x => x.InvoiceId == i.Id)
                    .Select(x => new InvoiceItemDetailDto
                    {
                        Id = x.Id,
                        ProductId = x.ProductId,
                        ItemName = x.ItemName,
                        Quantity = x.Quantity,
                        UnitPrice = x.UnitPrice,
                        VatRate = x.VatRate,
                        DiscountPercent = x.DiscountPercent,
                        LineSubtotal = x.LineSubtotal,
                        LineVatAmount = x.LineVatAmount,
                        LineTotal = x.LineTotal
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }

    // -- Vytvoří fakturu z objednávky --
    public async Task<CreateInvoiceFromOrderResult> CreateFromOrderAsync(int orderId)
    {
        // -- Načtení objednávky --
        var order = await _db.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null)
        {
            return new CreateInvoiceFromOrderResult
            {
                Success = false,
                ErrorCode = "ORDER_NOT_FOUND",
                Message = "Objednávka neexistuje."
            };
        }

        // -- Fakturu lze vytvořit jen z objednávky ve stavu Confirmed --
        if (order.Status == "Draft")
        {
            return new CreateInvoiceFromOrderResult
            {
                Success = false,
                ErrorCode = "ORDER_NOT_CONFIRMED",
                Message = "Fakturu lze vytvořit jen z objednávky ve stavu Confirmed."
            };
        }

        if (order.Status != "Confirmed")
        {
            return new CreateInvoiceFromOrderResult
            {
                Success = false,
                ErrorCode = "INVALID_ORDER_STATUS",
                Message = $"Fakturu nelze vytvořit z objednávky ve stavu {order.Status}."
            };
        }

        // -- Kontrola, zda už pro objednávku neexistuje faktura --
        var existingInvoice = await _db.Invoices
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.OrderId == orderId);

        if (existingInvoice != null)
        {
            return new CreateInvoiceFromOrderResult
            {
                Success = false,
                ErrorCode = "INVOICE_ALREADY_EXISTS",
                Message = $"Pro objednávku {orderId} už faktura existuje."
            };
        }

        // -- Načtení položek objednávky --
        var orderItems = await _db.OrderItems
            .AsNoTracking()
            .Where(i => i.OrderId == orderId)
            .ToListAsync();

        if (!orderItems.Any())
        {
            return new CreateInvoiceFromOrderResult
            {
                Success = false,
                ErrorCode = "NO_ORDER_ITEMS",
                Message = "Objednávka neobsahuje žádné položky."
            };
        }

        // -- Objednávka musí mít kladnou celkovou částku --
        if (order.TotalAmount <= 0)
        {
            return new CreateInvoiceFromOrderResult
            {
                Success = false,
                ErrorCode = "INVALID_ORDER_TOTAL",
                Message = "Fakturu nelze vytvořit z objednávky s nulovou nebo zápornou částkou."
            };
        }

        // -- Jednoduché vygenerování čísla faktury --
        var invoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMddHHmmss}-{orderId}";

        // -- Vytvoření hlavičky faktury --
        var invoice = new Invoice
        {
            InvoiceNumber = invoiceNumber,
            OrderId = order.Id,
            CustomerId = order.CustomerId,
            IssueDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(14),
            PaidDate = null,
            Status = "Issued",
            Subtotal = order.Subtotal,
            VatTotal = order.VatTotal,
            TotalAmount = order.TotalAmount,
            Currency = order.Currency,
            Note = $"Faktura vytvořená z objednávky {order.OrderNumber}",
            CreatedByUserId = order.CreatedByUserId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };

        _db.Invoices.Add(invoice);
        await _db.SaveChangesAsync();

        // -- Přenos položek objednávky do položek faktury --
        foreach (var item in orderItems)
        {
            var invoiceItem = new InvoiceItem
            {
                InvoiceId = invoice.Id,
                ProductId = item.ProductId,
                ItemName = item.ItemName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                VatRate = item.VatRate,
                DiscountPercent = item.DiscountPercent,
                LineSubtotal = item.LineSubtotal,
                LineVatAmount = item.LineVatAmount,
                LineTotal = item.LineTotal
            };

            _db.InvoiceItems.Add(invoiceItem);
        }

        await _db.SaveChangesAsync();

        return new CreateInvoiceFromOrderResult
        {
            Success = true,
            InvoiceId = invoice.Id,
            Message = "Faktura byla úspěšně vytvořena z objednávky."
        };
    }
}