using Dapper.Contrib.Extensions;

namespace Payments.Domain;

[Table("Payments")]
public class Payments
{
    public Payments()
    {
        
    }
    
    public Payments(
        int amount, 
        decimal fee,
        int clientId, 
        int companyId)
    {
        Amount = amount;
        ClientId = clientId;
        CompanyId = companyId;
        CalculatedFee = fee/100m * amount;
    }
    
    [Key]
    public int Id { get; set; }
    public int Amount { get; set; }
    public int ClientId { get; set; }
    public int CompanyId { get; set; }
    public decimal CalculatedFee { get; set; }
    [Write(false)]
    public DateTime CreatedAt { get; set; }
}