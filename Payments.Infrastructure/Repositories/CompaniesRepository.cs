using System.Data.Common;
using Dapper;
using Dapper.Contrib.Extensions;
using Payments.Domain;
using Payments.Domain.Repositories;
using Payments.Infrastructure.Repositories.Queries;

namespace Payments.Infrastructure.Repositories;

public class CompaniesRepository : ICompaniesRepository
{
    private readonly IDatabaseContext _databaseContext;

    public CompaniesRepository(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Companies> GetAsync(string documentNumber)
    {
        var company = await _databaseContext.Connection.QuerySingleOrDefaultAsync<Companies>(
            sql: CompaniesQueries.GetCompanyByDocumentNumber, param: new
            {
                documentNumber = new DbString
                {
                    Value = documentNumber,
                    IsAnsi = true
                }
            });

        return company;
    }

    public async Task<(decimal, int)> GetFeeAndIdAsync(string documentNumber)
    {
        var (fee, companyId) = await _databaseContext.Connection.QuerySingleOrDefaultAsync<(decimal,int)>(
            sql: CompaniesQueries.GetFeeAndIdByDocumentNumber, param: new
            {
                documentNumber = new DbString
                {
                    Value = documentNumber,
                    IsAnsi = true
                }
            });

        return (fee,companyId);    }

    public async Task<Companies> InsertAsync(Companies company)
    {
        DbTransaction dbTransaction = null;
        try
        {
            await using (dbTransaction = await _databaseContext.Connection.BeginTransactionAsync())
            {
                await _databaseContext.Connection.InsertAsync(company, dbTransaction);
                await dbTransaction.CommitAsync();
            }

            return company;
        }
        catch (Exception)
        {
            await dbTransaction?.RollbackAsync()!;
            throw;
        }
    }

    public async Task<Companies> UpdateAsync(Companies company)
    {
        DbTransaction dbTransaction = null;
        try
        {
            await using (dbTransaction = await _databaseContext.Connection.BeginTransactionAsync())
            {
                await _databaseContext.Connection.ExecuteAsync(sql:CompaniesQueries.UpdateCompany,param: new
                {
                    documentNumber = new DbString
                    {
                        Value = company.DocumentNumber,
                        IsAnsi = true
                    },
                    accountType = company.AccountType,
                    legalName = new DbString
                    {
                        Value = company.LegalName,
                        IsAnsi = true
                    },
                    bankAccount = new DbString
                    {
                        Value = company.BankAccount,
                        IsAnsi = true
                    },
                    bankCode = new DbString
                    {
                        Value = company.BankCode,
                        IsAnsi = true
                    },
                    isActive = company.IsActive,
                    fee = company.Fee
                }, dbTransaction);
                await dbTransaction.CommitAsync();
            }

            return company;
        }
        catch (Exception)
        {
            await dbTransaction?.RollbackAsync()!;
            throw;
        }
    }
}