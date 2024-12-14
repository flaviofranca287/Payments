using FluentAssertions;
using Moq;
using Payments.Application.ClientServices;
using Payments.Application.Dto;
using Payments.Domain;
using Payments.Domain.Repositories;
using Payments.Domain.ValueObjects;
using Xunit;

namespace Payments.UnitTests;

public class ClientServiceTest
{
    private ClientService _clientService = null!;
    private readonly Mock<IClientsRepository> _clientsRepositoryMock = new();

    [Fact]
    public async Task Given_NonExistentClient_When_Upsert_Then_ShouldInsert()
    {
        // Arrange
        var upsertOperation = CreateUpsertClientOperation();
        var newClient = upsertOperation.ToClients();

        _clientsRepositoryMock
            .Setup(repo => repo.GetAsync(upsertOperation.Plate))!
            .ReturnsAsync((Clients)null!);

        _clientsRepositoryMock
            .Setup(repo => repo.InsertAsync(It.IsAny<Clients>()))
            .ReturnsAsync(newClient);

        _clientsRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Clients>()));

        _clientService = new ClientService(_clientsRepositoryMock.Object);

        // Act
        var result = await _clientService.Upsert(upsertOperation);

        // Assert
        _clientsRepositoryMock.Verify(repo => repo.GetAsync(upsertOperation.Plate), Times.Once);
        _clientsRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<Clients>()), Times.Once);
        _clientsRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Clients>()), Times.Never);

        AssertUpsertClientOperation(result, upsertOperation);
    }

    [Fact]
    public async Task Given_AnExistentClient_When_Upsert_Then_ShouldCreateANewClient()
    {
        // Arrange
        var upsertOperation = CreateUpsertClientOperation();
        var existingClient = upsertOperation.ToClients();

        _clientsRepositoryMock
            .Setup(repo => repo.GetAsync(upsertOperation.Plate))
            .ReturnsAsync(existingClient);

        _clientsRepositoryMock
            .Setup(repo => repo.UpdateAsync(It.IsAny<Clients>()))
            .Returns(Task.CompletedTask);

        _clientsRepositoryMock
            .Setup(repo => repo.InsertAsync(It.IsAny<Clients>()))
            .ReturnsAsync(existingClient);

        _clientService = new ClientService(_clientsRepositoryMock.Object);

        // Act
        var result = await _clientService.Upsert(upsertOperation);

        // Assert
        _clientsRepositoryMock.Verify(repo => repo.GetAsync(upsertOperation.Plate), Times.Once);
        _clientsRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Clients>()), Times.Once);
        _clientsRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<Clients>()), Times.Once);

        AssertUpsertClientOperation(result, upsertOperation);
    }

    private static UpsertClientOperation CreateUpsertClientOperation()
    {
        return new UpsertClientOperation
        {
            Plate = "ABC123",
            ClientName = "AnyClient",
            IsActive = true,
            PaymentsInformation = new UpsertClientOperation.PaymentsInfo()
            {
                CardExpirationDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1),
                CardHolderName = "Any Card Holder",
                CardNumber = "4222111133332222",
                CardVerificationValue = 123,
                PaymentMethod = PaymentMethodEnum.Credit
            }
        };
    }

    private static void AssertUpsertClientOperation(UpsertClientOperation result, UpsertClientOperation upsertOperation)
    {
        result.Should().NotBeNull();
        result.Plate.Should().Be(upsertOperation.Plate);
        result.ClientName.Should().Be(upsertOperation.ClientName);
        result.IsActive.Should().Be(upsertOperation.IsActive);

        result.PaymentsInformation.Should().NotBeNull();
        result.PaymentsInformation!.CardHolderName.Should().Be(upsertOperation.PaymentsInformation!.CardHolderName);
        result.PaymentsInformation.CardExpirationDate.Should().Be(upsertOperation.PaymentsInformation.CardExpirationDate);
        result.PaymentsInformation.CardNumber.Should().Be(upsertOperation.PaymentsInformation.CardNumber);
        result.PaymentsInformation.CardVerificationValue.Should().Be(upsertOperation.PaymentsInformation.CardVerificationValue);
        result.PaymentsInformation.PaymentMethod.Should().Be(upsertOperation.PaymentsInformation.PaymentMethod);
    }
}