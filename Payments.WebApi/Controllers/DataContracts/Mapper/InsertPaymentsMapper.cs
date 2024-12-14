using Payments.Application.Dto;

namespace Payments.WebApi.Controllers.DataContracts.Mapper;

public static class InsertPaymentsMapper
{
    public static InsertPaymentOperation ToInsertPaymentOperation(this InsertPaymentRequest request) =>
        new(
            plate: request.Plate,
            amount: request.Amount,
            documentNumber: request.DocumentNumber
        );
}