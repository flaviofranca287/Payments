namespace Payments.WebApi.Controllers.DataContracts;

public record InsertPaymentRequest
{
    public string Plate { get; set; }
    public int Amount { get; set; }
    public string DocumentNumber { get; set; }
}