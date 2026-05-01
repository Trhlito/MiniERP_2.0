using FluentValidation;
using MiniERP.API.DTOs.Customers;

namespace MiniERP.API.Validators.Customers;

// Validátor pro úpravu zákazníka
public class UpdateCustomerRequestValidator : AbstractValidator<UpdateCustomerRequest>
{
    public UpdateCustomerRequestValidator()
    {
        // Človek - firma
        RuleFor(x => x.CustomerType)
            .NotEmpty()
            .Must(type => type == "Company" || type == "Person")
            .WithMessage("CustomerType musí být 'Company' nebo 'Person'.");

        // Název firmy
        RuleFor(x => x.CompanyName)
            .NotEmpty()
            .When(x => x.CustomerType == "Company")
            .WithMessage("Firma musí mít vyplněný CompanyName.");

        // Jméno
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .When(x => x.CustomerType == "Person")
            .WithMessage("Osoba musí mít vyplněné FirstName.");

        // Příjmení
        RuleFor(x => x.LastName)
            .NotEmpty()
            .When(x => x.CustomerType == "Person")
            .WithMessage("Osoba musí mít vyplněné LastName.");

        // format emailu
        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("Email není ve správném formátu.");
    }
}