namespace Payments.WebApi.Controllers.DataContracts;

public record UpsertCompanyRequest
{
    public string AccountType { get; set; }
    public string LegalName { get; set; }
    public string BankAccount { get; set; }
    public string BankCode { get; set; }
    public string IsActive { get; set; }
    public decimal Fee { get; set; }
}