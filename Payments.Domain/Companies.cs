using Dapper.Contrib.Extensions;
using Payments.Domain.ValueObjects;

namespace Payments.Domain;
[Table("Companies")]
public class Companies
{
    public Companies()
    {
        
    }
    public Companies(string documentNumber,
        AccountType accountType,
        string legalName,
        string bankAccount,
        string bankCode,
        bool isActive,
        decimal fee)
    {
        DocumentNumber = documentNumber;
        AccountType = accountType;
        LegalName = legalName;
        BankAccount = bankAccount;
        BankCode = bankCode;
        IsActive = isActive;
        Fee = fee;
    }
    [Key] 
    public int Id { get; init; }
    public string DocumentNumber { get; set; }
    public AccountType AccountType { get; set; }
    public string LegalName { get; set; }
    public string BankAccount { get; set; }
    public string BankCode { get; set; }
    public decimal Fee { get; set; }
    public bool IsActive { get; set; }
    [Write(false)]
    public DateTime CreatedAt { get; set; }
}