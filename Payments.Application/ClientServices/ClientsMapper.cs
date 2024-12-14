using Payments.Application.Dto;
using Payments.Domain;

namespace Payments.Application.ClientServices;

public static class ClientsMapper
{
    public static Clients ToClients(this UpsertClientOperation upsertClientOperation) =>
        new(
            plate: upsertClientOperation.Plate,
            clientName: upsertClientOperation.ClientName!,
            isActive: upsertClientOperation.IsActive,
            cardHolderName: upsertClientOperation.PaymentsInformation!.CardHolderName,
            cardExpirationDate: upsertClientOperation.PaymentsInformation.CardExpirationDate,
            cardNumber: long.Parse(upsertClientOperation.PaymentsInformation.CardNumber),
            cardVerificationValue: upsertClientOperation.PaymentsInformation.CardVerificationValue,
            paymentMethod: upsertClientOperation.PaymentsInformation.PaymentMethod);

    public static UpsertClientOperation ToUpsertClientOperation(this Clients clients)
    {
        return new UpsertClientOperation(
            Plate: clients.Plate,
            ClientName: clients.ClientName,
            IsActive: clients.IsActive,
            PaymentsInformation: new UpsertClientOperation.PaymentsInfo
            {
                CardHolderName = clients.CardHolderName,
                CardExpirationDate = clients.CardExpirationDate,
                CardNumber = clients.CardNumber.ToString(),
                CardVerificationValue = clients.CardVerificationValue,
                PaymentMethod = clients.PaymentMethod
            }
        );
    }

}