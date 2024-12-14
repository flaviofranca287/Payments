using Microsoft.AspNetCore.Mvc;

namespace Payments.WebApi.Controllers.DataContracts;

public record UpsertClientRequest
{
    public string ClientName { get; set; }
    public PaymentsInfo PaymentsInfo { get; set; }
}

public record PaymentsInfo
{
    public string CardHolderName { get; set; }
    public DateOnly CardExpirationDate { get; set; }
    public string CardNumber { get; set; }
    public string CardVerificationValue { get; set; }
    public string PaymentMethod { get; set; }
}