using System.Data.Common;
using Dapper.Contrib.Extensions;
using Payments.Domain.Repositories;

namespace Payments.Infrastructure.Repositories;

public class PaymentsRepository : IPaymentsRepository
{
    private readonly IDatabaseContext _databaseContext;

    public PaymentsRepository(IDatabaseContext databaseContext) => _databaseContext = databaseContext;

    public async Task<Domain.Payments> InsertAsync(Domain.Payments payment)
    {
        DbTransaction dbTransaction = null;
        try
        {
            await using (dbTransaction = await _databaseContext.Connection.BeginTransactionAsync())
            {
                await _databaseContext.Connection.InsertAsync(payment, dbTransaction);
                await dbTransaction.CommitAsync();
            }

            return payment;
        }
        catch (Exception)
        {
            await dbTransaction?.RollbackAsync()!;
            throw;
        }
    }
}