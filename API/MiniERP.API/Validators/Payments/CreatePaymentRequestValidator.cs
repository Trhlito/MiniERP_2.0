using FluentValidation;
using MiniERP.API.DTOs.Payments;

namespace MiniERP.API.Validators.Payments;

// -- Validátor pro vytvoření platby --
public class CreatePaymentRequestValidator : AbstractValidator<CreatePaymentRequest>
{
    public CreatePaymentRequestValidator()
    {
        // -- InvoiceId musí být větší než 0 --
        RuleFor(x => x.InvoiceId)
            .GreaterThan(0)
            .WithMessage("InvoiceId musí být větší než 0.");

        // -- Amount musí být větší než 0 --
        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount musí být větší než 0.");

        // -- PaymentMethod musí odpovídat databázi --
        RuleFor(x => x.PaymentMethod)
            .Must(x => new[] { "Cash", "Card", "Transfer" }.Contains(x))
            .WithMessage("PaymentMethod musí být Cash, Card nebo Transfer.");

        // -- ReferenceNumber může mít maximálně 100 znaků --
        RuleFor(x => x.ReferenceNumber)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.ReferenceNumber))
            .WithMessage("ReferenceNumber může mít maximálně 100 znaků.");

        // -- Note může mít maximálně 255 znaků --
        RuleFor(x => x.Note)
            .MaximumLength(255)
            .When(x => !string.IsNullOrWhiteSpace(x.Note))
            .WithMessage("Note může mít maximálně 255 znaků.");

        // -- CreatedByUserId musí být větší než 0 --
        RuleFor(x => x.CreatedByUserId)
            .GreaterThan(0)
            .WithMessage("CreatedByUserId musí být větší než 0.");
    }
}