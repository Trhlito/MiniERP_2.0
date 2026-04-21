using FluentValidation;
using MiniERP.API.DTOs.Products;

namespace MiniERP.API.Validators.Products;

// Validátor pro vytvoření nového produktu
public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        // Kontrola povinného Code
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("Code je povinný a může mít maximálně 50 znaků.");

        // Kontrola povinného Name
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Name je povinný a může mít maximálně 200 znaků.");

        // Kontrola hodnoty CategoryId
        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .WithMessage("CategoryId musí být větší než 0.");

        // Kontrola nezáporné PurchasePrice
        RuleFor(x => x.PurchasePrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage("PurchasePrice nesmí být záporná.");

        // Kontrola nezáporné SalePrice
        RuleFor(x => x.SalePrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage("SalePrice nesmí být záporná.");

        // Kontrola nezáporné VatRate
        RuleFor(x => x.VatRate)
            .GreaterThanOrEqualTo(0)
            .WithMessage("VatRate nesmí být záporná.");

        // Kontrola nezáporného MinimumStock
        RuleFor(x => x.MinimumStock)
            .GreaterThanOrEqualTo(0)
            .WithMessage("MinimumStock nesmí být záporný.");

        // Kontrola délky Unit
        RuleFor(x => x.Unit)
            .MaximumLength(20)
            .When(x => !string.IsNullOrWhiteSpace(x.Unit))
            .WithMessage("Unit může mít maximálně 20 znaků.");
    }
}