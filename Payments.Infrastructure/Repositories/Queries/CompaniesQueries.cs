namespace Payments.Infrastructure.Repositories.Queries;

public class CompaniesQueries
{
    public const string GetCompanyByDocumentNumber = @"
    SELECT DocumentNumber,
           AccountType,
           LegalName,
           BankAccount,
           BankCode,
           IsActive,
           CreatedAt,
           Fee
    FROM Companies
    WHERE DocumentNumber = @DocumentNumber";
    
    public const string GetFeeAndIdByDocumentNumber = @"
    SELECT 
           Fee,
           Id
    FROM Companies
    WHERE DocumentNumber = @DocumentNumber";
    
    public const string UpdateCompany = @"
    UPDATE Companies
    SET 
        AccountType = @AccountType,
        LegalName = @LegalName,
        BankAccount = @BankAccount,
        BankCode = @BankCode,
        IsActive = @IsActive,
        Fee = @Fee
    WHERE DocumentNumber = @DocumentNumber;
";
}