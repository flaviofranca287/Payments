using Payments.Domain.ValueObjects;

namespace Payments.Application.Dto;

public record struct UpsertClientOperation(
    string Plate,
    string? ClientName = null,
    bool IsActive = true,
    UpsertClientOperation.PaymentsInfo? PaymentsInformation = null)
{
    public string Plate { get; init; } = Plate;
    public string? ClientName { get; init; } = ClientName;
    public bool IsActive { get; init; } = IsActive;

    public record PaymentsInfo
    {
        public string CardHolderName { get; set; }
        public DateOnly CardExpirationDate { get; set; }
        public string CardNumber { get; set; }
        public int CardVerificationValue { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }
    }
}