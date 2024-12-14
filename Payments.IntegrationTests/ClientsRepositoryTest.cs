using Dapper;
using FluentAssertions;
using Payments.Domain;
using Payments.Domain.Repositories;
using Payments.Domain.ValueObjects;
using Payments.Infrastructure.Repositories;

namespace Payments.IntegrationTests;

public class ClientsRepositoryTest : BaseRepositoryTest
{
    private readonly IClientsRepository _clientsRepository;
    private readonly Clients _clients;

    public ClientsRepositoryTest() => _clientsRepository = new ClientsRepository(DatabaseContext);

    [Fact]
    public async Task Given_AClient_When_GetAsync_Then_ShouldReturnAsExpected()
    {
        // Arrange
        var databaseClient = CreateClients();

        CreateClientsInDatabase(databaseClient);

        // Act
        var client = await _clientsRepository.GetAsync(databaseClient.Plate);

        // Assert
        AssertClient(client, databaseClient, isActive: true);
    }

    [Fact]
    public async Task Given_AClient_When_InsertAsync_Then_ShouldBehaveAsExpected()
    {
        // Arrange
        var toBeInsertedClient = CreateClients();

        // Act
        var clients = await _clientsRepository.InsertAsync(toBeInsertedClient);

        // Assert
        var insertedClients = await _clientsRepository.GetAsync(toBeInsertedClient.Plate);
       
        AssertClient(insertedClients, toBeInsertedClient, isActive: true);
        clients.Should().BeEquivalentTo(toBeInsertedClient);
        
        DropClientsTable();
    }

    [Fact]
    public async Task Given_AClient_When_UpdateAsync_Then_ShouldDeactivateClient()
    {
        // Arrange
        var oldClient = CreateClients();

        CreateClientsInDatabase(oldClient);

        // Act
        await _clientsRepository.UpdateAsync(oldClient);

        // Assert
        var updatedClient = await _clientsRepository.GetAsync(oldClient.Plate);
        
        AssertClient(updatedClient, oldClient,isActive: false);
    }
    
    [Fact]
    public async Task Given_APlate_When_GetIdByPlate_Then_ShouldReturnId()
    {
        // Arrange
        var client = CreateClients();

        CreateClientsInDatabase(client);

        // Act
        await _clientsRepository.GetIdAsync(client.Plate);

        // Assert
        var id = await _clientsRepository.GetIdAsync(client.Plate);

        id.Should().BeGreaterThan(0);
    }
    
    private static void AssertClient(Clients clientToBeAsserted, Clients expectedClient, bool isActive)
    {
        clientToBeAsserted.ClientName.Should().Be(expectedClient.ClientName);
        clientToBeAsserted.Plate.Should().Be(expectedClient.Plate);
        clientToBeAsserted.IsActive.Should().Be(isActive);
        clientToBeAsserted.CreatedAt.Should().BeCloseTo(DateTime.UtcNow,TimeSpan.FromSeconds(5));
        clientToBeAsserted.CardHolderName.Should().Be(expectedClient.CardHolderName);
        clientToBeAsserted.CardExpirationDate.Should().Be(expectedClient.CardExpirationDate);
        clientToBeAsserted.CardNumber.Should().Be(expectedClient.CardNumber);
        clientToBeAsserted.CardVerificationValue.Should().Be(expectedClient.CardVerificationValue);
        clientToBeAsserted.PaymentMethod.Should().Be(expectedClient.PaymentMethod);
    }

    private static Clients CreateClients()
    {
        return new Clients
        {
            Plate = "ABC1234",
            ClientName = "FirstClient",
            CreatedAt = DateTime.UtcNow,
            IsActive = true,
            CardHolderName = "Any card holder",
            CardExpirationDate = DateOnly.FromDateTime(DateTime.UtcNow),
            CardNumber = 123456781234567812,
            CardVerificationValue = 123,
            PaymentMethod = PaymentMethodEnum.Credit
        };
    }
    
    private void CreateClientsInDatabase(Clients clients)
    {
        DataFactory.CreateOne("Clients", x =>
        {
            x.WithValue("Plate", clients.Plate);
            x.WithValue("ClientName", clients.ClientName);
            x.WithValue("IsActive", clients.IsActive);
            x.WithValue("CreatedAt", clients.CreatedAt);
            x.WithValue("CardHolderName", clients.CardHolderName);
            x.WithValue("CardExpirationDate", clients.CardExpirationDate);
            x.WithValue("CardNumber", clients.CardNumber);
            x.WithValue("CardVerificationValue", clients.CardVerificationValue);
            x.WithValue("PaymentMethod", clients.PaymentMethod);
        });
    }

    private async void DropClientsTable()
    {
        var query = "DELETE FROM Clients";
        await DatabaseContext.Connection.ExecuteAsync(query);
    }
}