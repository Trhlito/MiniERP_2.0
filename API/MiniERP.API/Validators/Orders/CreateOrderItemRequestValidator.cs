using FluentValidation;
using MiniERP.API.DTOs.Orders;

namespace MiniERP.API.Validators.Orders;

// -- Validátor pro položku objednávky --
public class CreateOrderItemRequestValidator : AbstractValidator<CreateOrderItemRequest>
{
    public CreateOrderItemRequestValidator()
    {
        // -- ProductId musí být větší než 0 --
        RuleFor(x => x.ProductId)
            .GreaterThan(0)
            .WithMessage("ProductId musí být větší než 0.");

        // -- ItemName je povinný --
        RuleFor(x => x.ItemName)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("ItemName je povinný a může mít maximálně 200 znaků.");

        // -- Quantity musí být větší než 0 --
        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity musí být větší než 0.");

        // -- UnitPrice nesmí být záporná --
        RuleFor(x => x.UnitPrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage("UnitPrice nesmí být záporná.");

        // -- VatRate nesmí být záporná --
        RuleFor(x => x.VatRate)
            .GreaterThanOrEqualTo(0)
            .WithMessage("VatRate nesmí být záporná.");

        // -- DiscountPercent nesmí být záporný --
        RuleFor(x => x.DiscountPercent)
            .GreaterThanOrEqualTo(0)
            .When(x => x.DiscountPercent.HasValue)
            .WithMessage("DiscountPercent nesmí být záporný.");
    }
}   