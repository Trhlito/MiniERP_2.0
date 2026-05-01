using FluentValidation;
using MiniERP.API.DTOs.Products;

namespace MiniERP.API.Validators.Products;

// Validátor vstupu pro vytvoření nového produktu.
public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        // Kód musí být vyplněný
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("Code je povinný a může mít maximálně 50 znaků.");

        // Název má omezenou délku
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Name je povinný a může mít maximálně 200 znaků.");

        // Produkt musí patřit do existující kategorie
        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .WithMessage("CategoryId musí být větší než 0.");

        // Nákupní cena nesmí být záporná
        RuleFor(x => x.PurchasePrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage("PurchasePrice nesmí být záporná.");

        // Prodejní cena nesmí být záporná
        RuleFor(x => x.SalePrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage("SalePrice nesmí být záporná.");

        // Sazba DPH nesmí být záporná
        RuleFor(x => x.VatRate)
            .GreaterThanOrEqualTo(0)
            .WithMessage("VatRate nesmí být záporná.");

        // Minimální zásoba slouží pro hlídání dostupnosti
        RuleFor(x => x.MinimumStock)
            .GreaterThanOrEqualTo(0)
            .WithMessage("MinimumStock nesmí být záporný.");

        // Jednotka má omezenou délku
        RuleFor(x => x.Unit)
            .MaximumLength(20)
            .When(x => !string.IsNullOrWhiteSpace(x.Unit))
            .WithMessage("Unit může mít maximálně 20 znaků.");
    }
}