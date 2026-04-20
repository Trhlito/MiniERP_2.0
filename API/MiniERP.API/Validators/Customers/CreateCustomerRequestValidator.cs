using FluentValidation;
using MiniERP.API.DTOs.Customers;

namespace MiniERP.API.Validators.Customers;

// -- Validátor pro vytvoření zákazníka --
public class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
{
    public CreateCustomerRequestValidator()
    {
        // -- CustomerType musí být Company nebo Person --
        RuleFor(x => x.CustomerType)
            .NotEmpty()
            .Must(type => type == "Company" || type == "Person")
            .WithMessage("CustomerType musí být 'Company' nebo 'Person'.");

        // -- Firma musí mít CompanyName --
        RuleFor(x => x.CompanyName)
            .NotEmpty()
            .When(x => x.CustomerType == "Company")
            .WithMessage("Firma musí mít vyplněný CompanyName.");

        // -- Osoba musí mít FirstName --
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .When(x => x.CustomerType == "Person")
            .WithMessage("Osoba musí mít vyplněné FirstName.");

        // -- Osoba musí mít LastName --
        RuleFor(x => x.LastName)
            .NotEmpty()
            .When(x => x.CustomerType == "Person")
            .WithMessage("Osoba musí mít vyplněné LastName.");

        // -- Email musí mít správný formát, pokud je vyplněn --
        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("Email není ve správném formátu.");
    }
}