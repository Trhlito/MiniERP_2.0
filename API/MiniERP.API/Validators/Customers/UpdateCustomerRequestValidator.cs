using FluentValidation;
using MiniERP.API.DTOs.Customers;

namespace MiniERP.API.Validators.Customers;

// Validátor pro úpravu zákazníka
public class UpdateCustomerRequestValidator : AbstractValidator<UpdateCustomerRequest>
{
    public UpdateCustomerRequestValidator()
    {
        // Povolené hodnoty pro CustomerType
        RuleFor(x => x.CustomerType)
            .NotEmpty()
            .Must(type => type == "Company" || type == "Person")
            .WithMessage("CustomerType musí být 'Company' nebo 'Person'.");

        // Povinný CompanyName pro firmu
        RuleFor(x => x.CompanyName)
            .NotEmpty()
            .When(x => x.CustomerType == "Company")
            .WithMessage("Firma musí mít vyplněný CompanyName.");

        // Povinný FirstName pro osobu
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .When(x => x.CustomerType == "Person")
            .WithMessage("Osoba musí mít vyplněné FirstName.");

        // Povinný LastName pro osobu
        RuleFor(x => x.LastName)
            .NotEmpty()
            .When(x => x.CustomerType == "Person")
            .WithMessage("Osoba musí mít vyplněné LastName.");

        // Kontrola formátu e-mailu
        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("Email není ve správném formátu.");
    }
}