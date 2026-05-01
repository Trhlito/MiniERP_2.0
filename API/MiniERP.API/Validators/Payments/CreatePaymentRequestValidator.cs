using FluentValidation;
using MiniERP.API.DTOs.Payments;

namespace MiniERP.API.Validators.Payments;

// Validátor vstupu pro vytvoření platby
public class CreatePaymentRequestValidator : AbstractValidator<CreatePaymentRequest>
{
    public CreatePaymentRequestValidator()
    {
        // Platba musí být navázaná na existující fakturu
        RuleFor(x => x.InvoiceId)
            .GreaterThan(0)
            .WithMessage("InvoiceId musí být větší než 0.");

        // Částka musí být kladná
        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount musí být větší než 0.");

        // Způsob platby musí odpovídat podporovaným typům
        RuleFor(x => x.PaymentMethod)
            .Must(x => new[] { "Cash", "Card", "Transfer" }.Contains(x))
            .WithMessage("PaymentMethod musí být Cash, Card nebo Transfer.");

        // Referenční číslo má omezenou délku
        RuleFor(x => x.ReferenceNumber)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.ReferenceNumber))
            .WithMessage("ReferenceNumber může mít maximálně 100 znaků.");

        // Kontrola délky poznámky
        RuleFor(x => x.Note)
            .MaximumLength(255)
            .When(x => !string.IsNullOrWhiteSpace(x.Note))
            .WithMessage("Note může mít maximálně 255 znaků.");

        // Platba musí mít evidovaného uživatele
        RuleFor(x => x.CreatedByUserId)
            .GreaterThan(0)
            .WithMessage("CreatedByUserId musí být větší než 0.");
    }
}