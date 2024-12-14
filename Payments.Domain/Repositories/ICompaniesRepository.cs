namespace Payments.Domain.Repositories;

public interface ICompaniesRepository
{
    Task<Companies> GetAsync(string documentNumber);
    Task<(decimal,int)> GetFeeAndIdAsync(string documentNumber);
    Task<Companies> InsertAsync(Companies company);
    Task<Companies> UpdateAsync(Companies companies);
}