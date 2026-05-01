using FluentValidation;
using MiniERP.API.DTOs.Customers;

namespace MiniERP.API.Validators.Customers;

// Validátor pro vytvoření zákazníka
public class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
{
    public CreateCustomerRequestValidator()
    {
        // ověření zákazníky __ zatím člověk - firma
        RuleFor(x => x.CustomerType)
            .NotEmpty()
            .Must(type => type == "Company" || type == "Person")
            .WithMessage("CustomerType musí být 'Company' nebo 'Person'.");

        // Povinný název firmy
        RuleFor(x => x.CompanyName)
            .NotEmpty()
            .When(x => x.CustomerType == "Company")
            .WithMessage("Firma musí mít vyplněný CompanyName.");

        // Povinné jméno
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .When(x => x.CustomerType == "Person")
            .WithMessage("Osoba musí mít vyplněné FirstName.");

        // Povinné příjmení
        RuleFor(x => x.LastName)
            .NotEmpty()
            .When(x => x.CustomerType == "Person")
            .WithMessage("Osoba musí mít vyplněné LastName.");

        // Základní kontrola formátu e-mailu
        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("Email není ve správném formátu.");
    }
}