using FluentValidation;
using MiniERP.API.DTOs.Stock;

namespace MiniERP.API.Validators.Stock;

// Validátor pro vytvoření skladového záznamu
public class CreateStockRequestValidator : AbstractValidator<CreateStockRequest>
{
    public CreateStockRequestValidator()
    {
        // Kontrola hodnoty WarehouseId
        RuleFor(x => x.WarehouseId)
            .GreaterThan(0)
            .WithMessage("WarehouseId musí být větší než 0.");

        // Kontrola hodnoty ProductId
        RuleFor(x => x.ProductId)
            .GreaterThan(0)
            .WithMessage("ProductId musí být větší než 0.");

        // Kontrola nezáporné hodnoty Quantity
        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Quantity nesmí být záporné.");

        // Kontrola nezáporné hodnoty ReservedQuantity
        RuleFor(x => x.ReservedQuantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("ReservedQuantity nesmí být záporné.");

        // Kontrola ReservedQuantity vůči Quantity
        RuleFor(x => x)
            .Must(x => x.ReservedQuantity <= x.Quantity)
            .WithMessage("ReservedQuantity nesmí být větší než Quantity.");
    }
}