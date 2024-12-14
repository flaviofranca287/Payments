using Payments.Application.Dto;
using Payments.Domain;
using Payments.Domain.ValueObjects;

namespace Payments.Application.CompanyServices;

public static class CompaniesMapper
{
    public static Companies ToCompanies(this UpsertCompanyOperation upsertCompanyOperation) =>
        new(
            accountType: upsertCompanyOperation.AccountType == "checking_account"
                ? AccountType.CheckingAccount
                : AccountType.SavingsAccount,
            legalName: upsertCompanyOperation.LegalName,
            bankAccount: upsertCompanyOperation.BankAccount,
            bankCode: upsertCompanyOperation.BankCode,
            isActive: upsertCompanyOperation.IsActive,
            documentNumber: upsertCompanyOperation.DocumentNumber,
            fee: upsertCompanyOperation.Fee
        );

    public static UpsertCompanyOperation ToUpsertCompanyOperation(this Companies companies) =>
        new(DocumentNumber: companies.DocumentNumber,
            AccountType: companies.AccountType.ToString(), 
            LegalName: companies.LegalName,
            BankAccount: companies.BankAccount, 
            BankCode: companies.BankCode, 
            IsActive: companies.IsActive,
            Fee: companies.Fee);
}