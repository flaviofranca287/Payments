using FluentValidation;
using Payments.WebApi.Controllers.DataContracts;

namespace Payments.WebApi.Controllers.Validators;

public class InsertPaymentRequestValidator: AbstractValidator<InsertPaymentRequest>
{
    public InsertPaymentRequestValidator()
    {
        RuleFor(it => it.Plate)
            .NotEmpty().WithMessage("Plate can not be empty.")
            .Length(7).WithMessage("Invalid Plate Length.");
        
        RuleFor(it => it.DocumentNumber)
            .NotEmpty().WithMessage("DocumentNumber cannot be empty.")
            .Must(doc => doc.Length == 11 || doc.Length == 14)
            .WithMessage("DocumentNumber must be 11 (CPF) or 14 (CNPJ) characters long.");

        RuleFor(it => it.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0.");
    }
}