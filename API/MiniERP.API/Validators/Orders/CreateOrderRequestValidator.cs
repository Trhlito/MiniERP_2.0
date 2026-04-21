using FluentValidation;
using MiniERP.API.DTOs.Orders;

namespace MiniERP.API.Validators.Orders;

// Validátor pro vytvoření objednávky
public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        // Kontrola povinného OrderNumber
        RuleFor(x => x.OrderNumber)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("OrderNumber je povinné a může mít maximálně 50 znaků.");

        // Kontrola hodnoty CustomerId
        RuleFor(x => x.CustomerId)
            .GreaterThan(0)
            .WithMessage("CustomerId musí být větší než 0.");

        // Kontrola povolených hodnot Status
        RuleFor(x => x.Status)
            .Must(status => new[] { "Draft", "Confirmed", "Completed", "Cancelled" }.Contains(status))
            .WithMessage("Status musí být: Draft, Confirmed, Completed nebo Cancelled.");

        // Kontrola povinné Currency
        RuleFor(x => x.Currency)
            .NotEmpty()
            .MaximumLength(10)
            .WithMessage("Currency je povinná a může mít maximálně 10 znaků.");

        // Kontrola hodnoty CreatedByUserId
        RuleFor(x => x.CreatedByUserId)
            .GreaterThan(0)
            .WithMessage("CreatedByUserId musí být větší než 0.");

        // Kontrola existence položek objednávky
        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("Objednávka musí obsahovat alespoň jednu položku.");

        // Validace jednotlivých položek
        RuleForEach(x => x.Items)
            .SetValidator(new CreateOrderItemRequestValidator());

        // Kontrola délky Note
        RuleFor(x => x.Note)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Note))
            .WithMessage("Note může mít maximálně 500 znaků.");
    }
}