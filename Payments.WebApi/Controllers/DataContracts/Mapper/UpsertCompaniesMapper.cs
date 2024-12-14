using Payments.Application.Dto;

namespace Payments.WebApi.Controllers.DataContracts.Mapper;

public static class UpsertCompaniesMapper
{
    public static UpsertCompanyOperation ToUpsertCompanyOperation(this UpsertCompanyRequest request, string documentNumber)
    {
        return new UpsertCompanyOperation(
            DocumentNumber: documentNumber,
            AccountType: request.AccountType,
            LegalName: request.LegalName,
            BankAccount: request.BankAccount,
            BankCode: request.BankCode,
            IsActive: bool.Parse(request.IsActive),
            Fee: request.Fee
        );
    }
}