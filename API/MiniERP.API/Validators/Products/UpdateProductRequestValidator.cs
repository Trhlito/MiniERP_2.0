using FluentValidation;
using MiniERP.API.DTOs.Products;

namespace MiniERP.API.Validators.Products;

// -- Validátor pro úpravu produktu --
public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductRequestValidator()
    {
        // -- Code je povinný --
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("Pole 'Code' nesmí být prázdné.");

        // -- Name je povinný --
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Pole 'Name' nesmí být prázdné.");

        // -- CategoryId musí být větší než 0 --
        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .WithMessage("CategoryId musí být větší než 0.");

        // -- PurchasePrice nesmí být záporná --
        RuleFor(x => x.PurchasePrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage("PurchasePrice nesmí být záporná.");

        // -- SalePrice nesmí být záporná --
        RuleFor(x => x.SalePrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage("SalePrice nesmí být záporná.");

        // -- VatRate nesmí být záporná --
        RuleFor(x => x.VatRate)
            .GreaterThanOrEqualTo(0)
            .WithMessage("VatRate nesmí být záporná.");

        // -- MinimumStock nesmí být záporný --
        RuleFor(x => x.MinimumStock)
            .GreaterThanOrEqualTo(0)
            .WithMessage("MinimumStock nesmí být záporný.");

        // -- Unit může mít maximálně 20 znaků --
        RuleFor(x => x.Unit)
            .MaximumLength(20)
            .When(x => !string.IsNullOrWhiteSpace(x.Unit))
            .WithMessage("Unit může mít maximálně 20 znaků.");
    }
}