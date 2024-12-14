using Payments.Application.Dto;

namespace Payments.Application.CompanyServices;

public interface ICompanyService
{
    Task<UpsertCompanyOperation> Upsert(UpsertCompanyOperation upsertOperation);
}