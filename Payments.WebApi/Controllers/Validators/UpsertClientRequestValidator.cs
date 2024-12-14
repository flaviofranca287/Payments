using FluentValidation;
using Payments.WebApi.Controllers.DataContracts;

namespace Payments.WebApi.Controllers.Validators;

public class UpsertClientRequestValidator : AbstractValidator<UpsertClientRequest>
{
    public UpsertClientRequestValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        ClassLevelCascadeMode = CascadeMode.Stop;
        
        RuleFor(it => it.ClientName)
            .NotEmpty().WithMessage("ClientName can not be empty.");
        
        RuleFor(it => it.PaymentsInfo)
            .NotEmpty().WithMessage("PaymentsInfo can not be empty.");
        
        RuleFor(it => it.PaymentsInfo.CardHolderName)
            .NotEmpty().WithMessage("CardHolderName can not be empty.");
        
        RuleFor(it => it.PaymentsInfo.CardExpirationDate)
            .NotEqual(default(DateOnly))
            .WithMessage("CreatedAt cannot be the default value.")
            .Must(x => x >= DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("CardExpirationDate must be less than or equal to today.");
        
        RuleFor(it => it.PaymentsInfo.CardNumber)
            .NotEmpty().WithMessage("Card number cannot be empty.")
            .Matches(@"^\d+$").WithMessage("Card number must contain only numeric characters.")
            .Must(cardNumber => cardNumber.Length == 13 || cardNumber.Length == 16)
            .WithMessage("Card number must be 13 or 16 digits long.");
        
        RuleFor(it => it.PaymentsInfo.CardVerificationValue)
            .Length(3).WithMessage("CardVerificationValue must be a valid value.");
        
        RuleFor(it => it.PaymentsInfo.PaymentMethod)
            .Must(x => x == "credit_card").WithMessage("For now, PaymentMethod must be equals to 'credit_card'.");
    }
}