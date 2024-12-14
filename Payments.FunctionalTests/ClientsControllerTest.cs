using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Payments.WebApi;
using Payments.WebApi.Controllers.DataContracts;
using Xunit;
using FluentAssertions;

namespace Payments.FunctionalTests;

public class ClientsControllerTest : ControllerBaseTest
{
    public ClientsControllerTest(WebApplicationFactory<Program> factory) : base(factory) { }

    [Fact]
    public async Task Given_ValidRequest_When_Upsert_Then_ShouldReturnCreated()
    {
        // Arrange
        var plate = "ABC1234";
        var request = CreateUpsertClientRequest();

        // Act
        var response = await PutAsync($"/clients/register/{plate}", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Given_InvalidPlate_When_Upsert_Then_ShouldReturnBadRequest()
    {
        // Arrange
        var plate = "123";
        var request = CreateUpsertClientRequest();

        // Act
        var response = await PutAsync($"/clients/register/{plate}", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Invalid plate");
    }
    
    [Theory]
    [InlineData("123", "Test Client", "4111111111111111", "123", "credit_card", "Invalid plate")] 
    [InlineData("ABC1234", "", "4111111111111111", "123", "credit_card", "ClientName can not be empty.")] 
    [InlineData("ABC1234", "Test Client", "", "123", "credit_card", "Card number cannot be empty.")]
    [InlineData("ABC1234", "Test Client", "4111111111", "123", "credit_card", "Card number must be 13 or 16 digits long.")]
    [InlineData("ABC1234", "Test Client", "4111111111111111", "12", "credit_card", "CardVerificationValue must be a valid value.")] 
    [InlineData("ABC1234", "Test Client", "4111111111111111", "123", "debit_card", "For now, PaymentMethod must be equals to 'credit_card'.")]
    public async Task Given_InvalidRequest_When_Upsert_Then_ShouldReturnBadRequest(
        string plate,
        string clientName,
        string cardNumber,
        string cardVerificationValue,
        string paymentMethod,
        string expectedErrorMessage)
    {
        // Arrange
        var request = new UpsertClientRequest
        {
            ClientName = clientName,
            PaymentsInfo = new PaymentsInfo
            {
                CardHolderName = "Card Holder",
                CardExpirationDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1)),
                CardNumber = cardNumber,
                CardVerificationValue = cardVerificationValue,
                PaymentMethod = paymentMethod
            }
        };

        // Act
        var response = await PutAsync($"/clients/register/{plate}", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain(expectedErrorMessage);
    }
    

}
