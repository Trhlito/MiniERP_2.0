using FluentValidation;
using MiniERP.API.DTOs.Orders;

namespace MiniERP.API.Validators.Orders;

// Validátor pro úpravu hlavičky objednávky
public class UpdateOrderRequestValidator : AbstractValidator<UpdateOrderRequest>
{
    public UpdateOrderRequestValidator()
    {
        // Kontrola povolených hodnot Status
        RuleFor(x => x.Status)
            .Must(status => new[] { "Draft", "Confirmed", "Completed", "Cancelled" }.Contains(status))
            .WithMessage("Status musí být: Draft, Confirmed, Completed nebo Cancelled.");

        // povinná měna
        RuleFor(x => x.Currency)
            .NotEmpty()
            .MaximumLength(10)
            .WithMessage("Currency je povinná a může mít maximálně 10 znaků.");

        // kontrola délky poznámky
        RuleFor(x => x.Note)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Note))
            .WithMessage("Note může mít maximálně 500 znaků.");
    }
}