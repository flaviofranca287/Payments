namespace Payments.Domain.Repositories;

public interface IClientsRepository
{
    Task<Clients> GetAsync(string plate);
    Task<int> GetIdAsync(string plate);

    Task<Clients> InsertAsync(Clients client);
    
    Task UpdateAsync(Clients client);

}