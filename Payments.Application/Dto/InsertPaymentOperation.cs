namespace Payments.Application.Dto;

public record struct InsertPaymentOperation
{
    public InsertPaymentOperation(string plate, int amount, string documentNumber)
    {
        Plate = plate;
        Amount = amount;
        DocumentNumber = documentNumber;
    }
    public string Plate { get; set; }
    public int Amount { get; set; }
    public string DocumentNumber { get; set; }
}