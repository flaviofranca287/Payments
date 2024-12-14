using Payments.Application.Dto;

namespace Payments.Application.ClientServices;

public interface IClientsService
{
    Task<UpsertClientOperation> Upsert(UpsertClientOperation upsertOperation);
}