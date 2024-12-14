using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Payments.WebApi;
using Payments.WebApi.Controllers.DataContracts;
using Xunit;
using FluentAssertions;
using Payments.IntegrationTests;
using StrangerData;
using StrangerData.SqlServer;

namespace Payments.FunctionalTests;

public class PaymentsControllerTest : ControllerBaseTest
{
    public PaymentsControllerTest(WebApplicationFactory<Program> factory) : base(factory) { }

    [Theory]
    [InlineData("", 100, "12345678901", "Plate can not be empty.")]
    [InlineData("ABC123", 100, "12345678901", "Invalid Plate Length.")]
    [InlineData("ABC1234", 100, "", "DocumentNumber cannot be empty.")]
    [InlineData("ABC1234", 100, "12345", "DocumentNumber must be 11 (CPF) or 14 (CNPJ) characters long.")]
    [InlineData("ABC1234", -10, "12345678901", "Amount must be greater than 0.")]
    public async Task Given_InvalidRequest_When_CreatePayment_Then_ShouldReturnBadRequest(
        string plate,
        int amount,
        string documentNumber,
        string expectedErrorMessage)
    {
        // Arrange
        var request = new InsertPaymentRequest
        {
            Plate = plate,
            Amount = amount,
            DocumentNumber = documentNumber
        };

        // Act
        var response = await PostAsync("/payments", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain(expectedErrorMessage);
    }

    [Fact]
    public async Task Given_ValidRequest_When_CreatePayment_Then_ShouldReturnCreated()
    {
        // Arrange
        var plate = "ABC1234";
        var clientRequest = CreateUpsertClientRequest();
        (await PutAsync($"/clients/register/{plate}", clientRequest)).StatusCode.Should().Be(HttpStatusCode.Created);
        
        var documentNumber = "12345678901";
        var companyRequest = CreateValidUpsertCompanyRequest();
        (await PutAsync($"/companies/register/{documentNumber}", companyRequest)).StatusCode.Should().Be(HttpStatusCode.Created);
        
        var request = new InsertPaymentRequest
        {
            Plate = plate,
            Amount = 1000,
            DocumentNumber = documentNumber
        };

        // Act
        var response = await PostAsync("/payments", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}
