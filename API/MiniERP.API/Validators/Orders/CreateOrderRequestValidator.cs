using FluentValidation;
using MiniERP.API.DTOs.Orders;

namespace MiniERP.API.Validators.Orders;

// Validátor vstupu pro vytvoření objednávky
public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        // Číslo objednávky je povinné a má omezenou délku
        RuleFor(x => x.OrderNumber)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("OrderNumber je povinné a může mít maximálně 50 znaků.");

        // Povinnost validního identifikátoru
        RuleFor(x => x.CustomerId)
            .GreaterThan(0)
            .WithMessage("CustomerId musí být větší než 0.");

        // // Kontrola Statusu
        RuleFor(x => x.Status)
            .Must(status => new[] { "Draft", "Confirmed", "Completed", "Cancelled" }.Contains(status))
            .WithMessage("Status musí být: Draft, Confirmed, Completed nebo Cancelled.");

        // Měna je povinná a má omezenou délku
        RuleFor(x => x.Currency)
            .NotEmpty()
            .MaximumLength(10)
            .WithMessage("Currency je povinná a může mít maximálně 10 znaků.");

        // Autor obj. musí být uživatel systému
        RuleFor(x => x.CreatedByUserId)
            .GreaterThan(0)
            .WithMessage("CreatedByUserId musí být větší než 0.");

        // Objednávka musí něco obsahovat
        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("Objednávka musí obsahovat alespoň jednu položku.");

        // Každá položka má vlastní valid. pravidla
        RuleForEach(x => x.Items)
            .SetValidator(new CreateOrderItemRequestValidator());

        // kontrola délky poznámky
        RuleFor(x => x.Note)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Note))
            .WithMessage("Note může mít maximálně 500 znaků.");
    }
}