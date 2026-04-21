using FluentValidation;
using MiniERP.API.DTOs.Stock;

namespace MiniERP.API.Validators.Stock;

// Validátor pro úpravu skladového záznamu
public class UpdateStockRequestValidator : AbstractValidator<UpdateStockRequest>
{
    public UpdateStockRequestValidator()
    {
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