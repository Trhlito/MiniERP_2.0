using FluentValidation;
using MiniERP.API.DTOs.Products;

namespace MiniERP.API.Validators.Products;

// Validátor vstupu pro úpravu produktu.
public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductRequestValidator()
    {
        // Kód produktu nesmí být null
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("Pole 'Code' nesmí být prázdné.");

        // Název produktu má omezenou délku
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Pole 'Name' nesmí být prázdné.");

        // Produkt musí zůstat navázaný na platnou kategorii
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

        // DPH nesmí být záporné
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