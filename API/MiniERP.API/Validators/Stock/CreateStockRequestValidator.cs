using FluentValidation;
using MiniERP.API.DTOs.Stock;

namespace MiniERP.API.Validators.Stock;

// Validátor vstupu pro vytvoření skladového záznamu
public class CreateStockRequestValidator : AbstractValidator<CreateStockRequest>
{
    public CreateStockRequestValidator()
    {
        // Sklad musí být validní identifikátor
        RuleFor(x => x.WarehouseId)
            .GreaterThan(0)
            .WithMessage("WarehouseId musí být větší než 0.");

        // Produkt musí být validní identifikátor
        RuleFor(x => x.ProductId)
            .GreaterThan(0)
            .WithMessage("ProductId musí být větší než 0.");

        // Množství nesmí být záporné
        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Quantity nesmí být záporné.");

        // Rezervované množství nesmí být záporné
        RuleFor(x => x.ReservedQuantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("ReservedQuantity nesmí být záporné.");

        // Rezervace nesmí být vyšší než celkové množství na skladě
        RuleFor(x => x)
            .Must(x => x.ReservedQuantity <= x.Quantity)
            .WithMessage("ReservedQuantity nesmí být větší než Quantity.");
    }
}