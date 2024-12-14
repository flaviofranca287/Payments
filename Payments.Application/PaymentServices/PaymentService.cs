using Payments.Application.Dto;
using Payments.Domain.Exceptions;
using Payments.Domain.Repositories;

namespace Payments.Application.PaymentServices;

public class PaymentService : IPaymentService
{
    private readonly IPaymentsRepository _paymentsRepository;
    private readonly IClientsRepository _clientsRepository;
    private readonly ICompaniesRepository _companiesRepository;

    public PaymentService(IPaymentsRepository paymentsRepository, IClientsRepository clientsRepository, ICompaniesRepository companiesRepository)
    {
        _paymentsRepository = paymentsRepository;
        _clientsRepository = clientsRepository;
        _companiesRepository = companiesRepository;
    }
    
    public async Task Insert(InsertPaymentOperation operation)
    {
        var clientId = await _clientsRepository.GetIdAsync(operation.Plate);
        var (fee, companyId) = await _companiesRepository.GetFeeAndIdAsync(operation.DocumentNumber);

        if (clientId == 0 || companyId == 0)
        {
            throw new UnregisteredException("Unregistered client or company.");
        }
        var payment = operation.ToPayments(fee,clientId,companyId);

        await _paymentsRepository.InsertAsync(payment);
    }
}