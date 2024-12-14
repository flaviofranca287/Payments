using Dapper.Contrib.Extensions;
using Payments.Domain.ValueObjects;

namespace Payments.Domain;

[Table("Clients")]
public class Clients
{
    public Clients()
    {
    }

    public Clients(string plate, 
        string clientName, 
        bool isActive,
        string cardHolderName,
        DateOnly cardExpirationDate,
        long cardNumber,
        int cardVerificationValue,
        PaymentMethodEnum paymentMethod)
    {
        Plate = plate;
        ClientName = clientName;
        IsActive = isActive;
        CardHolderName = cardHolderName;
        CardExpirationDate = cardExpirationDate;
        CardNumber = cardNumber;
        CardVerificationValue = cardVerificationValue;
        PaymentMethod = paymentMethod;
    }
    
    [Key] 
    public int Id { get; set; }
    public string Plate { get; set; }
    public string ClientName { get; set; }
    public string CardHolderName { get; set; }
    public DateOnly CardExpirationDate { get; set; }
    public long CardNumber { get; set; }
    public int CardVerificationValue { get; set; }
    public PaymentMethodEnum PaymentMethod { get; set; }
    public bool IsActive { get; set; }
    [Write(false)]
    public DateTime CreatedAt { get; set; }
}