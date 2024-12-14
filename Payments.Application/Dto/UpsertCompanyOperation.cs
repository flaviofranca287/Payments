namespace Payments.Application.Dto;

public record struct UpsertCompanyOperation(
    string DocumentNumber,
    string AccountType,
    string LegalName,
    string BankAccount,
    string BankCode,
    bool IsActive,
    decimal Fee
)
{
    public string DocumentNumber { get; init; } = DocumentNumber;
    public string AccountType { get; init; } = AccountType;
    public string LegalName { get; init; } = LegalName;
    public string BankAccount { get; init; } = BankAccount;
    public string BankCode { get; init; } = BankCode;
    public decimal Fee { get; init; } = Fee;
    public bool IsActive { get; init; } = IsActive;
}