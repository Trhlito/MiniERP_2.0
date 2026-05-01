using FluentValidation;
using MiniERP.API.DTOs.Orders;

namespace MiniERP.API.Validators.Orders;

// Validátor vstupu pro položku objednávky
public class CreateOrderItemRequestValidator : AbstractValidator<CreateOrderItemRequest>
{
    public CreateOrderItemRequestValidator()
    {
        // Produkt musí být validní identifikátor
        RuleFor(x => x.ProductId)
            .GreaterThan(0)
            .WithMessage("ProductId musí být větší než 0.");

        // Název položky je povinný a má omezenou délku
        RuleFor(x => x.ItemName)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("ItemName je povinný a může mít maximálně 200 znaků.");

        // Množství musí být kladné
        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity musí být větší než 0.");

        // Cena nesmí být záporná
        RuleFor(x => x.UnitPrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage("UnitPrice nesmí být záporná.");

        // DPH nesmí být záporné
        RuleFor(x => x.VatRate)
            .GreaterThanOrEqualTo(0)
            .WithMessage("VatRate nesmí být záporná.");

        // Sleva se kontroluje pokud je vyplněná
        RuleFor(x => x.DiscountPercent)
            .GreaterThanOrEqualTo(0)
            .When(x => x.DiscountPercent.HasValue)
            .WithMessage("DiscountPercent nesmí být záporný.");
    }
}