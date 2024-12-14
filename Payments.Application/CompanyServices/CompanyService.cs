using Payments.Application.Dto;
using Payments.Domain.Repositories;

namespace Payments.Application.CompanyServices;

public class CompanyService : ICompanyService
{
    private readonly ICompaniesRepository _companiesRepository;

    public CompanyService(ICompaniesRepository companiesRepository)
    {
        _companiesRepository = companiesRepository;
    }

    public async Task<UpsertCompanyOperation> Upsert(UpsertCompanyOperation upsertOperation)
    {
        var company = await _companiesRepository.GetAsync(upsertOperation.DocumentNumber);

        if (company == null)
        {
            company = upsertOperation.ToCompanies();
            var insertedCompany = await _companiesRepository.InsertAsync(company);
            return insertedCompany.ToUpsertCompanyOperation();
        }

        company = upsertOperation.ToCompanies();
        var updatedCompany = await _companiesRepository.UpdateAsync(company);
        return updatedCompany.ToUpsertCompanyOperation();
    }
}