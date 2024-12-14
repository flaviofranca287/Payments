using FluentValidation;
using Payments.WebApi.Controllers.DataContracts;

namespace Payments.WebApi.Controllers.Validators;

public class UpsertCompanyRequestValidator : AbstractValidator<UpsertCompanyRequest>
{
    public UpsertCompanyRequestValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(it => it.AccountType)
            .NotEmpty().WithMessage("AccountType cannot be empty.")
            .Must(type => type == "savings_account" || type == "checking_account")
            .WithMessage("AccountType must be either 'savings_account' or 'checking_account'.");

        RuleFor(it => it.LegalName)
            .NotEmpty()
            .WithMessage("LegalName can not be empty.")
            .MaximumLength(100).WithMessage("LegalName must be less than or equal to 100 characters.");

        RuleFor(it => it.BankAccount)
            .NotEmpty().WithMessage("BankAccount cannot be empty.")
            .MaximumLength(14).WithMessage("BankAccount must be less than or equal to 14 digits.")
            .Matches(@"^\d+$").WithMessage("BankAccount must contain only numeric digits.");


        RuleFor(it => it.BankCode)
            .NotEmpty().WithMessage("BankCode can not be empty.")
            .Length(3).WithMessage("BankCode must have only 3 digits.")
            .Matches(@"^\d+$").WithMessage("BankCode must contain only numeric digits.");

        RuleFor(it => it.IsActive)
            .NotEmpty().WithMessage("IsActive cannot be empty.")
            .Must(value => value == "true" || value == "false")
            .WithMessage("IsActive must be either 'true' or 'false' as a string.");

        RuleFor(it => it.Fee)
            .InclusiveBetween(0, 100).WithMessage("Fee must be in between 0 and 100.");
    }
}