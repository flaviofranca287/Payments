using Moq;
using Payments.Application.Dto;
using Payments.Application.PaymentServices;
using Payments.Domain.Repositories;
using Xunit;

namespace Payments.UnitTests;

public class PaymentServiceTest
{
    private PaymentService _paymentService = null!;
    private readonly Mock<IPaymentsRepository> _paymentsRepositoryMock = new();
    private readonly Mock<IClientsRepository> _clientsRepositoryMock = new();
    private readonly Mock<ICompaniesRepository> _companiesRepositoryMock = new();

    [Fact]
    public async Task Given_ValidOperation_When_Insert_Then_ShouldInsertPayment()
    {
        // Arrange
        var operation = CreateInsertPaymentOperation();
        const int clientId = 1;
        const int companyId = 2;
        const decimal fee = 2.5m;
        
        var payment = operation.ToPayments(fee, clientId, companyId);

        _clientsRepositoryMock
            .Setup(repo => repo.GetIdAsync(operation.Plate))
            .ReturnsAsync(clientId);

        _companiesRepositoryMock
            .Setup(repo => repo.GetFeeAndIdAsync(operation.DocumentNumber))
            .ReturnsAsync((fee, companyId));

        _paymentsRepositoryMock
            .Setup(repo => repo.InsertAsync(It.IsAny<Domain.Payments>()))
            .ReturnsAsync(payment);

        _paymentService = new PaymentService(
            _paymentsRepositoryMock.Object,
            _clientsRepositoryMock.Object,
            _companiesRepositoryMock.Object
        );

        // Act
        await _paymentService.Insert(operation);

        // Assert
        _clientsRepositoryMock.Verify(repo => repo.GetIdAsync(operation.Plate), Times.Once);
        _companiesRepositoryMock.Verify(repo => repo.GetFeeAndIdAsync(operation.DocumentNumber), Times.Once);
        _paymentsRepositoryMock.Verify(repo => repo.InsertAsync(It.Is<Domain.Payments>(p =>
            p.Amount == operation.Amount &&
            p.ClientId == clientId &&
            p.CompanyId == companyId &&
            p.CalculatedFee == fee / 100m * operation.Amount
        )), Times.Once);
    }

    private static InsertPaymentOperation CreateInsertPaymentOperation()
    {
        return new InsertPaymentOperation
        {
            Plate = "ABC123",
            DocumentNumber = "12345678901",
            Amount = 1000
        };
    }
}
