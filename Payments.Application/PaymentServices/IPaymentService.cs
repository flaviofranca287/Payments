using Payments.Application.Dto;

namespace Payments.Application.PaymentServices;

public interface IPaymentService
{
    Task Insert(InsertPaymentOperation operation);
}