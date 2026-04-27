using System.Data;
using Microsoft.EntityFrameworkCore;
using MiniERP.API.DTOs.Invoices;
using MiniERP.API.Services.Interfaces;
using MiniERP.Data;
using MiniERP.Data.Entities;
using MiniERP.API.Services.Results;

namespace MiniERP.API.Services.Implementations;

// Implementace služby pro faktury
public class InvoiceService : IInvoiceService
{
    // Databázový kontext
    private readonly ApplicationDbContext _db;

    public InvoiceService(ApplicationDbContext db)
    {
        _db = db;
    }

    // Načtení seznamu faktur
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

    // Načtení detailu faktury podle ID
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

                // Načtení položek faktury
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

    // Vytvoření faktury z objednávky //
    public async Task<CreateInvoiceFromOrderResult> CreateFromOrderAsync(int orderId)
    {
        // Načtení objednávky //
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

        // Kontrola stavu Draft //
        if (order.Status == "Draft")
        {
            return new CreateInvoiceFromOrderResult
            {
                Success = false,
                ErrorCode = "ORDER_NOT_CONFIRMED",
                Message = "Fakturu lze vytvořit jen z objednávky ve stavu Confirmed."
            };
        }

        // Kontrola povoleného stavu objednávky //
        if (order.Status != "Confirmed")
        {
            return new CreateInvoiceFromOrderResult
            {
                Success = false,
                ErrorCode = "INVALID_ORDER_STATUS",
                Message = $"Fakturu nelze vytvořit z objednávky ve stavu {order.Status}."
            };
        }

        try
        {
            // Databázové připojení z EF Core kontextu //
            var connection = _db.Database.GetDbConnection();

            // Otevření připojení při zavřeném stavu //
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            // Vytvoření databázového příkazu pro stored procedure //
            await using var command = connection.CreateCommand();
            command.CommandText = "dbo.sp_CreateInvoiceFromOrder";
            command.CommandType = CommandType.StoredProcedure;

            // Parametr ID objednávky //
            var orderIdParameter = command.CreateParameter();
            orderIdParameter.ParameterName = "@OrderId";
            orderIdParameter.Value = orderId;
            command.Parameters.Add(orderIdParameter);

            // Parametr ID uživatele //
            var createdByUserIdParameter = command.CreateParameter();
            createdByUserIdParameter.ParameterName = "@CreatedByUserId";
            createdByUserIdParameter.Value = order.CreatedByUserId;
            command.Parameters.Add(createdByUserIdParameter);

            // Spuštění procedury a načtení výsledku //
            await using var reader = await command.ExecuteReaderAsync();

            // Kontrola vráceného výsledku //
            if (!await reader.ReadAsync())
            {
                return new CreateInvoiceFromOrderResult
                {
                    Success = false,
                    ErrorCode = "INVOICE_CREATE_FAILED",
                    Message = "Procedura nevrátila výsledek vytvořené faktury."
                };
            }

            // Načtení ID faktury z výsledku procedury //
            var invoiceIdOrdinal = reader.GetOrdinal("InvoiceId");
            var invoiceId = reader.GetInt32(invoiceIdOrdinal);

            return new CreateInvoiceFromOrderResult
            {
                Success = true,
                InvoiceId = invoiceId,
                Message = "Faktura byla úspěšně vytvořena z objednávky."
            };
        }
        catch (Exception ex)
        {
            return new CreateInvoiceFromOrderResult
            {
                Success = false,
                ErrorCode = "CREATE_INVOICE_FAILED",
                Message = ex.Message
            };
        }
    }
}