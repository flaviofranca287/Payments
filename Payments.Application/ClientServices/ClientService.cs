using Payments.Application.Dto;
using Payments.Domain.Repositories;

namespace Payments.Application.ClientServices;

public class ClientService : IClientsService
{
    private readonly IClientsRepository _clientsRepository;
    
    public ClientService(IClientsRepository clientsRepository)
    {
        _clientsRepository = clientsRepository;
    }
    
    public async Task<UpsertClientOperation> Upsert(UpsertClientOperation upsertOperation)
    {
        var oldClient = await _clientsRepository.GetAsync(upsertOperation.Plate);
        
        if (oldClient == null) return await Insert(upsertOperation);

        await _clientsRepository.UpdateAsync(oldClient);
        return await Insert(upsertOperation);
    }

    private async Task<UpsertClientOperation> Insert(UpsertClientOperation upsertOperation)
    {
        var client = upsertOperation.ToClients();
        var insertedClient = await _clientsRepository.InsertAsync(client);
        return insertedClient.ToUpsertClientOperation();
    }
}