using Dapper;
using FluentAssertions;
using Payments.Domain.Repositories;
using Payments.Infrastructure.Repositories;

namespace Payments.IntegrationTests;

public class PaymentsRepositoryTest : BaseRepositoryTest
{
    private readonly IPaymentsRepository _paymentsRepository;

    public PaymentsRepositoryTest() => _paymentsRepository = new PaymentsRepository(DatabaseContext);

    [Fact]
    public async Task Given_APayment_When_InsertAsync_Then_ShouldPersistCorrectly()
    {
        // Arrange
        var payment = CreatePayment();

        // Act
        var insertedPayment = await _paymentsRepository.InsertAsync(payment);

        // Assert
        var fetchedPayment = await GetPaymentFromDatabase(insertedPayment.Id);

        AssertPayment(fetchedPayment, payment);
        insertedPayment.Should().BeEquivalentTo(payment, options => options.Excluding(p => p.CreatedAt));

        DropPaymentsTable();
    }

    private static Domain.Payments CreatePayment()
    {
        var amount = 1000;
        var fee = 2.5m;
        var clientId = 1;
        var companyId = 1;

        return new Domain.Payments(amount, fee, clientId, companyId)
        {
            CreatedAt = DateTime.UtcNow
        };
    }

    private static void AssertPayment(Domain.Payments actual, Domain.Payments expected)
    {
        actual.Amount.Should().Be(expected.Amount);
        actual.ClientId.Should().Be(expected.ClientId);
        actual.CompanyId.Should().Be(expected.CompanyId);
        actual.CalculatedFee.Should().Be(expected.CalculatedFee);
        actual.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    private async Task<Domain.Payments> GetPaymentFromDatabase(int paymentId)
    {
        var query = "SELECT Id, Amount, ClientId, CompanyId, CalculatedFee, CreatedAt FROM Payments WHERE Id = @Id";
        return await DatabaseContext.Connection.QuerySingleOrDefaultAsync<Domain.Payments>(query, new { Id = paymentId });
    }

    private async void DropPaymentsTable()
    {
        var query = "DELETE FROM Payments";
        await DatabaseContext.Connection.ExecuteAsync(query);
    }
}
