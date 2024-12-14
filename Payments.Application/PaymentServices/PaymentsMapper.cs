using Payments.Application.Dto;

namespace Payments.Application.PaymentServices;

public static class PaymentsMapper
{
    public static Domain.Payments ToPayments(this InsertPaymentOperation operation, decimal fee, int clientId, int companyId) =>
        new(
            amount: operation.Amount,
            fee: fee,
            clientId: clientId,
            companyId: companyId
        );
}