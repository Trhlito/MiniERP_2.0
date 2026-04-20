using FluentValidation;
using MiniERP.API.DTOs.Orders;

namespace MiniERP.API.Validators.Orders;

// -- Validátor pro vytvoření objednávky --
public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        // -- OrderNumber je povinné --
        RuleFor(x => x.OrderNumber)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("OrderNumber je povinné a může mít maximálně 50 znaků.");

        // -- CustomerId musí být větší než 0 --
        RuleFor(x => x.CustomerId)
            .GreaterThan(0)
            .WithMessage("CustomerId musí být větší než 0.");

        // -- Status je povinný --
        RuleFor(x => x.Status)
            .Must(status => new[] { "Draft", "Confirmed", "Completed", "Cancelled" }.Contains(status))
            .WithMessage("Status musí být: Draft, Confirmed, Completed nebo Cancelled.");

        // -- Currency je povinná --
        RuleFor(x => x.Currency)
            .NotEmpty()
            .MaximumLength(10)
            .WithMessage("Currency je povinná a může mít maximálně 10 znaků.");

        // -- CreatedByUserId musí být větší než 0 --
        RuleFor(x => x.CreatedByUserId)
            .GreaterThan(0)
            .WithMessage("CreatedByUserId musí být větší než 0.");

        // -- Objednávka musí mít alespoň jednu položku --
        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("Objednávka musí obsahovat alespoň jednu položku.");

        // -- Validace každé položky --
        RuleForEach(x => x.Items)
            .SetValidator(new CreateOrderItemRequestValidator());

        // -- Note může mít maximálně 500 znaků --
        RuleFor(x => x.Note)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Note))
            .WithMessage("Note může mít maximálně 500 znaků.");
    }
}