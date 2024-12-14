namespace Payments.Domain.Repositories;

public interface IPaymentsRepository
{
    Task<Payments> InsertAsync(Payments payment);
}