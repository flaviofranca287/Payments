using Payments.Application.Dto;
using Payments.Domain.ValueObjects;

namespace Payments.WebApi.Controllers.DataContracts.Mapper;

public static class UpsertClientsMapper
{
    public static UpsertClientOperation ToUpsertClientOperation(this UpsertClientRequest request, string plate) =>
        new(
            Plate: plate.ToLowerInvariant(),
            ClientName: request.ClientName,
            PaymentsInformation: request.PaymentsInfo.ToPaymentsInfoOperation()
        );

    private static UpsertClientOperation.PaymentsInfo ToPaymentsInfoOperation(this PaymentsInfo paymentsInfo)
    {
        return new UpsertClientOperation.PaymentsInfo
        {
            CardHolderName = paymentsInfo.CardHolderName,
            CardExpirationDate = paymentsInfo.CardExpirationDate,
            CardNumber = paymentsInfo.CardNumber,
            CardVerificationValue = int.TryParse(paymentsInfo.CardVerificationValue, out var cvv) ? cvv : 0,
            PaymentMethod = PaymentMethodEnum.Credit, // For now, we only accept credit cards
        };
    }
}
