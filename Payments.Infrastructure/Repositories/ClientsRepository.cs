using System.Data.Common;
using Dapper;
using Dapper.Contrib.Extensions;
using Payments.Domain;
using Payments.Domain.Repositories;
using Payments.Infrastructure.Repositories.Queries;

namespace Payments.Infrastructure.Repositories;

public class ClientsRepository : IClientsRepository
{
    private readonly IDatabaseContext _databaseContext;

    public ClientsRepository(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Clients> GetAsync(string plate)
    {
        var client = await _databaseContext.Connection.QuerySingleOrDefaultAsync<Clients>(
            sql: ClientsQueries.GetClientByPlate, param: new
            {
                plate = new DbString
                {
                    Value = plate,
                    IsAnsi = true
                }
            });

        return client;
    }

    public async Task<int> GetIdAsync(string plate)
    {
        var id = await _databaseContext.Connection.QuerySingleOrDefaultAsync<int>(
            sql: ClientsQueries.GetIdByPlate, param: new
            {
                plate = new DbString
                {
                    Value = plate,
                    IsAnsi = true
                }
            });

        return id;    }

    public async Task<Clients> InsertAsync(Clients client)
    {
        DbTransaction dbTransaction = null;
        try
        {
            await using (dbTransaction = await _databaseContext.Connection.BeginTransactionAsync())
            {
                await _databaseContext.Connection.InsertAsync(client, dbTransaction);
                await dbTransaction.CommitAsync();
            }

            return client;
        }
        catch (Exception)
        {
            await dbTransaction?.RollbackAsync()!;
            throw;
        }
    }

    public async Task UpdateAsync(Clients client)
    {
        DbTransaction dbTransaction = null;
        try
        {
            await using (dbTransaction = await _databaseContext.Connection.BeginTransactionAsync())
            {
                await _databaseContext.Connection.ExecuteAsync(sql:ClientsQueries.DeactivateClient,param: new
                {
                    plate = new DbString
                    {
                        Value = client.Plate,
                        IsAnsi = true
                    }
                }, dbTransaction);
                await dbTransaction.CommitAsync();
            }
        }
        catch (Exception)
        {
            await dbTransaction?.RollbackAsync()!;
            throw;
        }
    }
}