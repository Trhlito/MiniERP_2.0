using Microsoft.EntityFrameworkCore;
using MiniERP.API.DTOs.Orders;
using MiniERP.API.Services.Interfaces;
using MiniERP.Data;
using MiniERP.Data.Entities;
using MiniERP.API.Services.Results;

namespace MiniERP.API.Services.Implementations;

// -- Implementace služby pro objednávky --
public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _db;

    public OrderService(ApplicationDbContext db)
    {
        _db = db;
    }

    // -- Vrátí seznam objednávek --
    public async Task<List<OrderListItemDto>> GetAllAsync()
    {
        return await _db.Orders
            .AsNoTracking()
            .OrderByDescending(o => o.OrderDate)
            .Select(o => new OrderListItemDto
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                CustomerId = o.CustomerId,
                OrderDate = o.OrderDate,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
                Currency = o.Currency
            })
            .ToListAsync();
    }

    // -- Vrátí detail objednávky podle ID včetně položek --
    public async Task<OrderDetailDto?> GetByIdAsync(int id)
    {
        return await _db.Orders
            .AsNoTracking()
            .Where(o => o.Id == id)
            .Select(o => new OrderDetailDto
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                CustomerId = o.CustomerId,
                OrderDate = o.OrderDate,
                RequiredDate = o.RequiredDate,
                Status = o.Status,
                Subtotal = o.Subtotal,
                VatTotal = o.VatTotal,
                TotalAmount = o.TotalAmount,
                Currency = o.Currency,
                Note = o.Note,
                CreatedByUserId = o.CreatedByUserId,
                CreatedAt = o.CreatedAt,
                UpdatedAt = o.UpdatedAt,

                // -- Mapování položek objednávky --
                Items = _db.OrderItems
                    .Where(i => i.OrderId == o.Id)
                    .Select(i => new OrderItemDetailDto
                    {
                        Id = i.Id,
                        ProductId = i.ProductId,
                        ItemName = i.ItemName,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        VatRate = i.VatRate,
                        DiscountPercent = i.DiscountPercent,
                        LineSubtotal = i.LineSubtotal,
                        LineVatAmount = i.LineVatAmount,
                        LineTotal = i.LineTotal
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }

    // -- Vytvoří novou objednávku včetně položek a vrátí její ID --
    public async Task<int> CreateAsync(CreateOrderRequest request)
    {
        // -- Kontrola existence zákazníka --
        var customerExists = await _db.Customers
            .AsNoTracking()
            .AnyAsync(c => c.Id == request.CustomerId);

        if (!customerExists)
        {
            throw new Exception($"Zákazník s ID {request.CustomerId} neexistuje.");
        }

        // -- Kontrola, že objednávka obsahuje položky --
        if (request.Items == null || !request.Items.Any())
        {
            throw new Exception("Objednávka musí obsahovat alespoň jednu položku.");
        }

        // -- Seznam ID produktů z requestu --
        var requestedProductIds = request.Items
            .Select(i => i.ProductId)
            .Distinct()
            .ToList();

        // -- Načtení existujících produktů z databáze --
        var existingProducts = await _db.Products
            .AsNoTracking()
            .Where(p => requestedProductIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id);

        // -- Kontrola existence všech produktů --
        var missingProductIds = requestedProductIds
            .Where(productId => !existingProducts.ContainsKey(productId))
            .ToList();

        if (missingProductIds.Any())
        {
            throw new Exception(
                $"Některé produkty neexistují. Chybějící ProductId: {string.Join(", ", missingProductIds)}");
        }

        // -- Výpočet součtů objednávky --
        decimal subtotal = 0m;
        decimal vatTotal = 0m;
        decimal totalAmount = 0m;

        // -- Vytvoření hlavičky objednávky --
        var order = new Order
        {
            OrderNumber = request.OrderNumber,
            CustomerId = request.CustomerId,
            OrderDate = request.OrderDate,
            RequiredDate = request.RequiredDate,
            Status = request.Status,
            Currency = request.Currency,
            Note = request.Note,
            CreatedByUserId = request.CreatedByUserId,
            CreatedAt = DateTime.UtcNow
        };

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        // -- Vytvoření položek objednávky --
        foreach (var item in request.Items)
        {
            // -- Načtení produktu z již ověřených dat --
            var product = existingProducts[item.ProductId];

            // -- Pokud ItemName není vyplněný, doplníme ho z produktu --
            var itemName = string.IsNullOrWhiteSpace(item.ItemName)
                ? product.Name
                : item.ItemName.Trim();

            var discountPercent = item.DiscountPercent ?? 0m;

            // -- Výpočet mezisoučtu po slevě --
            var lineSubtotal = item.Quantity * item.UnitPrice;
            lineSubtotal = lineSubtotal * (1 - (discountPercent / 100m));

            // -- Výpočet DPH řádku --
            var lineVatAmount = lineSubtotal * (item.VatRate / 100m);

            // -- Výpočet celku řádku --
            var lineTotal = lineSubtotal + lineVatAmount;

            var orderItem = new OrderItem
            {
                OrderId = order.Id,
                ProductId = item.ProductId,
                ItemName = itemName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                VatRate = item.VatRate,
                DiscountPercent = item.DiscountPercent,
                LineSubtotal = lineSubtotal,
                LineVatAmount = lineVatAmount,
                LineTotal = lineTotal
            };

            _db.OrderItems.Add(orderItem);

            subtotal += lineSubtotal;
            vatTotal += lineVatAmount;
            totalAmount += lineTotal;
        }

        // -- Uložení položek --
        await _db.SaveChangesAsync();

        // -- Doplnění součtů do hlavičky objednávky --
        order.Subtotal = subtotal;
        order.VatTotal = vatTotal;
        order.TotalAmount = totalAmount;
        order.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return order.Id;
    }

    // -- Upraví hlavičku objednávky podle ID --
    public async Task<bool> UpdateAsync(int id, UpdateOrderRequest request)
    {
        // -- Načtení objednávky --
        var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return false;
        }

        // -- Pokud je objednávka Confirmed, povolíme jen omezené změny --
        if (order.Status == "Confirmed")
        {
            // -- Nepovolit ruční změnu statusu potvrzené objednávky --
            if (request.Status != order.Status)
            {
                throw new Exception(
                    $"Objednávku {order.OrderNumber} ve stavu Confirmed nelze ručně přepnout na jiný status.");
            }

            // -- Nepovolit změnu měny u potvrzené objednávky --
            if (request.Currency != order.Currency)
            {
                throw new Exception(
                    $"Objednávce {order.OrderNumber} ve stavu Confirmed nelze změnit měnu.");
            }

            // -- U Confirmed objednávky povolíme jen změnu poznámky a required date --
            order.RequiredDate = request.RequiredDate;
            order.Note = request.Note;
            order.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        // -- U Draft objednávky povolíme běžný update --
        if (order.Status == "Draft")
        {
            order.RequiredDate = request.RequiredDate;
            order.Status = request.Status;
            order.Currency = request.Currency;
            order.Note = request.Note;
            order.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        // -- Ostatní stavy zatím nepovolujeme upravovat --
        throw new Exception(
            $"Objednávku {order.OrderNumber} ve stavu {order.Status} zatím nelze upravovat.");
    }

    // -- Smaže objednávku podle ID včetně položek --
    public async Task<bool> DeleteAsync(int id)
    {
        // -- Najdeme objednávku --
        var order = await _db.Orders
            .FirstOrDefaultAsync(o => o.Id == id);

        // -- Pokud neexistuje --
        if (order == null)
        {
            return false;
        }

        // -- Nepovolit smazání potvrzené objednávky --
        if (order.Status == "Confirmed")
        {
            throw new Exception(
                $"Objednávku {order.OrderNumber} nelze smazat, protože je ve stavu Confirmed.");
        }

        // -- Nepovolit smazání objednávky, ke které existuje faktura --
        var hasInvoice = await _db.Invoices
            .AsNoTracking()
            .AnyAsync(i => i.OrderId == order.Id);

        if (hasInvoice)
        {
            throw new Exception(
                $"Objednávku {order.OrderNumber} nelze smazat, protože k ní existuje faktura.");
        }

        // -- Načtení položek objednávky --
        var items = await _db.OrderItems
            .Where(i => i.OrderId == id)
            .ToListAsync();

        // -- Smazání položek a hlavičky --
        _db.OrderItems.RemoveRange(items);
        _db.Orders.Remove(order);

        await _db.SaveChangesAsync();

        return true;
    }

    // -- Provede rezervaci skladu pro objednávku --
    public async Task<ReserveStockResult> ReserveStockAsync(int orderId)
    {
        // -- Načtení objednávky --
        var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null)
        {
            return new ReserveStockResult
            {
                Success = false,
                ErrorCode = "ORDER_NOT_FOUND",
                Message = "Objednávka neexistuje."
            };
        }

        // -- Pokud už je objednávka Confirmed, rezervace už byla provedena --
        if (order.Status == "Confirmed")
        {
            return new ReserveStockResult
            {
                Success = false,
                ErrorCode = "ALREADY_RESERVED",
                Message = "Objednávka už má rezervovaný sklad."
            };
        }

        // -- Rezervaci dovolíme jen pro Draft --
        if (order.Status != "Draft")
        {
            return new ReserveStockResult
            {
                Success = false,
                ErrorCode = "INVALID_STATUS",
                Message = "Rezervaci skladu lze provést jen pro objednávku ve stavu Draft."
            };
        }

        // -- Načtení položek objednávky --
        var orderItems = await _db.OrderItems
            .Where(i => i.OrderId == orderId)
            .ToListAsync();

        if (!orderItems.Any())
        {
            return new ReserveStockResult
            {
                Success = false,
                ErrorCode = "NO_ITEMS",
                Message = "Objednávka neobsahuje žádné položky."
            };
        }

        // -- Transakce pro bezpečné provedení celé operace --
        using var transaction = await _db.Database.BeginTransactionAsync();

        // -- Nejprve kontrola dostupnosti všech položek --
        foreach (var item in orderItems)
        {
            // -- Pro první verzi hledáme sklad podle ProductId --
            var stock = await _db.Stock
                .FirstOrDefaultAsync(s => s.ProductId == item.ProductId);

            if (stock == null)
            {
                return new ReserveStockResult
                {
                    Success = false,
                    ErrorCode = "STOCK_NOT_FOUND",
                    Message = $"Pro produkt {item.ProductId} neexistuje skladový záznam."
                };
            }

            // -- Výpočet volného množství --
            var availableQuantity = stock.Quantity - stock.ReservedQuantity;

            if (availableQuantity < item.Quantity)
            {
                return new ReserveStockResult
                {
                    Success = false,
                    ErrorCode = "INSUFFICIENT_STOCK",
                    Message = $"Nedostatek skladových zásob pro produkt {item.ProductId}."
                };
            }
        }

        // -- Pokud vše prošlo, provedeme rezervaci a zapíšeme stock movement --
        foreach (var item in orderItems)
        {
            var stock = await _db.Stock
                .FirstOrDefaultAsync(s => s.ProductId == item.ProductId);

            if (stock == null)
            {
                return new ReserveStockResult
                {
                    Success = false,
                    ErrorCode = "STOCK_NOT_FOUND",
                    Message = $"Pro produkt {item.ProductId} neexistuje skladový záznam."
                };
            }

            // -- Uložení hodnot před změnou --
            var quantityBefore = stock.Quantity;
            var reservedBefore = stock.ReservedQuantity;

            // -- Provedení rezervace --
            stock.ReservedQuantity += item.Quantity;
            stock.LastUpdatedAt = DateTime.UtcNow;

            // -- Uložení hodnot po změně --
            var quantityAfter = stock.Quantity;
            var reservedAfter = stock.ReservedQuantity;

            // -- Vytvoření auditního záznamu pohybu skladu --
            var stockMovement = new StockMovement
            {
                StockId = stock.Id,
                WarehouseId = stock.WarehouseId,
                ProductId = stock.ProductId,
                MovementType = "RESERVE",
                Quantity = item.Quantity,
                QuantityBefore = quantityBefore,
                QuantityAfter = quantityAfter,
                ReservedBefore = reservedBefore,
                ReservedAfter = reservedAfter,
                ReferenceType = "Order",
                ReferenceId = order.Id,
                Note = $"Rezervace skladu pro objednávku {order.OrderNumber}",
                CreatedByUserId = order.CreatedByUserId,
                CreatedAt = DateTime.UtcNow
            };

            _db.StockMovements.Add(stockMovement);
        }

        // -- Změna stavu objednávky na Confirmed --
        order.Status = "Confirmed";
        order.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        await transaction.CommitAsync();

        return new ReserveStockResult
        {
            Success = true,
            Message = "Rezervace skladu byla úspěšně provedena."
        };
    }

    // -- Uvolní rezervaci skladu pro objednávku --
    public async Task<ReserveStockResult> ReleaseStockAsync(int orderId)
    {
        // -- Načtení objednávky --
        var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null)
        {
            return new ReserveStockResult
            {
                Success = false,
                ErrorCode = "ORDER_NOT_FOUND",
                Message = "Objednávka neexistuje."
            };
        }

        // -- Pokud už je Draft, není co uvolnit --
        if (order.Status == "Draft")
        {
            return new ReserveStockResult
            {
                Success = false,
                ErrorCode = "NOT_RESERVED",
                Message = "Objednávka nemá žádnou rezervaci."
            };
        }

        // -- Povolený stav je jen Confirmed --
        if (order.Status != "Confirmed")
        {
            return new ReserveStockResult
            {
                Success = false,
                ErrorCode = "INVALID_STATUS",
                Message = "Uvolnění lze provést jen pro Confirmed."
            };
        }

        // -- Načtení položek objednávky --
        var orderItems = await _db.OrderItems
            .Where(i => i.OrderId == orderId)
            .ToListAsync();

        if (!orderItems.Any())
        {
            return new ReserveStockResult
            {
                Success = false,
                ErrorCode = "NO_ITEMS",
                Message = "Objednávka neobsahuje žádné položky."
            };
        }

        // -- Transakce pro bezpečné provedení celé operace --
        using var transaction = await _db.Database.BeginTransactionAsync();

        // -- Nejprve zkontrolujeme, že je co uvolnit --
        foreach (var item in orderItems)
        {
            var stock = await _db.Stock
                .FirstOrDefaultAsync(s => s.ProductId == item.ProductId);

            if (stock == null)
            {
                return new ReserveStockResult
                {
                    Success = false,
                    ErrorCode = "STOCK_NOT_FOUND",
                    Message = $"Pro produkt {item.ProductId} neexistuje skladový záznam."
                };
            }

            if (stock.ReservedQuantity < item.Quantity)
            {
                return new ReserveStockResult
                {
                    Success = false,
                    ErrorCode = "INVALID_RESERVED_QUANTITY",
                    Message = $"U produktu {item.ProductId} není dost rezervovaného množství pro uvolnění."
                };
            }
        }

        // -- Pokud vše prošlo, uvolníme rezervace a zapíšeme stock movement --
        foreach (var item in orderItems)
        {
            var stock = await _db.Stock
                .FirstOrDefaultAsync(s => s.ProductId == item.ProductId);

            if (stock == null)
            {
                return new ReserveStockResult
                {
                    Success = false,
                    ErrorCode = "STOCK_NOT_FOUND",
                    Message = $"Pro produkt {item.ProductId} neexistuje skladový záznam."
                };
            }

            // -- Uložení hodnot před změnou --
            var quantityBefore = stock.Quantity;
            var reservedBefore = stock.ReservedQuantity;

            // -- Uvolnění rezervace --
            stock.ReservedQuantity -= item.Quantity;
            stock.LastUpdatedAt = DateTime.UtcNow;

            // -- Uložení hodnot po změně --
            var quantityAfter = stock.Quantity;
            var reservedAfter = stock.ReservedQuantity;

            // -- Vytvoření auditního záznamu pohybu skladu --
            var stockMovement = new StockMovement
            {
                StockId = stock.Id,
                WarehouseId = stock.WarehouseId,
                ProductId = stock.ProductId,
                MovementType = "RELEASE",
                Quantity = item.Quantity,
                QuantityBefore = quantityBefore,
                QuantityAfter = quantityAfter,
                ReservedBefore = reservedBefore,
                ReservedAfter = reservedAfter,
                ReferenceType = "Order",
                ReferenceId = order.Id,
                Note = $"Uvolnění rezervace skladu pro objednávku {order.OrderNumber}",
                CreatedByUserId = order.CreatedByUserId,
                CreatedAt = DateTime.UtcNow
            };

            _db.StockMovements.Add(stockMovement);
        }

        // -- Vrácení objednávky zpět do Draft --
        order.Status = "Draft";
        order.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        await transaction.CommitAsync();

        return new ReserveStockResult
        {
            Success = true,
            Message = "Rezervace skladu byla úspěšně uvolněna."
        };
    }
}