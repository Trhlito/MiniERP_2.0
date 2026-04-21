using FluentValidation;
using MiniERP.API.DTOs.Orders;

namespace MiniERP.API.Validators.Orders;

// Validátor pro položku objednávky
public class CreateOrderItemRequestValidator : AbstractValidator<CreateOrderItemRequest>
{
    public CreateOrderItemRequestValidator()
    {
        // Kontrola hodnoty ProductId
        RuleFor(x => x.ProductId)
            .GreaterThan(0)
            .WithMessage("ProductId musí být větší než 0.");

        // Kontrola povinného ItemName
        RuleFor(x => x.ItemName)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("ItemName je povinný a může mít maximálně 200 znaků.");

        // Kontrola kladné hodnoty Quantity
        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity musí být větší než 0.");

        // Kontrola nezáporné UnitPrice
        RuleFor(x => x.UnitPrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage("UnitPrice nesmí být záporná.");

        // Kontrola nezáporné VatRate
        RuleFor(x => x.VatRate)
            .GreaterThanOrEqualTo(0)
            .WithMessage("VatRate nesmí být záporná.");

        // Kontrola nezáporného DiscountPercent
        RuleFor(x => x.DiscountPercent)
            .GreaterThanOrEqualTo(0)
            .When(x => x.DiscountPercent.HasValue)
            .WithMessage("DiscountPercent nesmí být záporný.");
    }
}