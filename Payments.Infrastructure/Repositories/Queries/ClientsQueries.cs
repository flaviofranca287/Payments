namespace Payments.Infrastructure.Repositories.Queries;

public static class ClientsQueries
{
    public const string GetClientByPlate = @"
    SELECT Plate,
           ClientName,
           IsActive,
           CreatedAt,
           CardHolderName,
           CardExpirationDate,
           CardNumber,
           CardVerificationValue,
           PaymentMethod
    FROM Clients
    WHERE Plate = @Plate";
    
    public const string GetIdByPlate = @"
    SELECT Id
    FROM Clients
    WHERE Plate = @Plate";
    
    public const string DeactivateClient = @"
    UPDATE Clients
    SET IsActive = 0
    WHERE Plate = @Plate 
    AND IsActive = 1
    ";
}