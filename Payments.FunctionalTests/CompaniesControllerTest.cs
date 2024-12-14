using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Payments.WebApi;
using Payments.WebApi.Controllers.DataContracts;
using Xunit;

namespace Payments.FunctionalTests;

public class CompaniesControllerTest : ControllerBaseTest
{
    public CompaniesControllerTest(WebApplicationFactory<Program> factory) : base(factory) { }

    [Theory]
    [InlineData("123", "savings_account", "Legal Name", "987654321", "001", "true", 10, "Invalid DocumentNumber. It must be either 11 or 14 numeric digits.")] 
    [InlineData("12345678901", "", "Legal Name", "987654321", "001", "true", 10, "AccountType cannot be empty.")] 
    [InlineData("12345678901", "invalid_type", "Legal Name", "987654321", "001", "true", 10, "AccountType must be either 'savings_account' or 'checking_account'.")] 
    [InlineData("12345678901", "savings_account", "", "987654321", "001", "true", 10, "LegalName can not be empty.")] 
    [InlineData("12345678901", "checking_account", "Legal Name", "", "001", "true", 10, "BankAccount cannot be empty.")] 
    [InlineData("12345678901", "savings_account", "Legal Name", "123456789012345", "001", "true", 10, "BankAccount must be less than or equal to 14 digits.")]
    [InlineData("12345678901", "savings_account", "Legal Name", "invalid", "001", "true", 10, "BankAccount must contain only numeric digits.")] 
    [InlineData("12345678901", "savings_account", "Legal Name", "987654321", "", "true", 10, "BankCode can not be empty.")]
    [InlineData("12345678901", "savings_account", "Legal Name", "987654321", "12", "true", 10, "BankCode must have only 3 digits.")]
    [InlineData("12345678901", "savings_account", "Legal Name", "987654321", "abc", "true", 10, "BankCode must contain only numeric digits.")] 
    [InlineData("12345678901", "savings_account", "Legal Name", "987654321", "001", "", 10, "IsActive cannot be empty.")]
    [InlineData("12345678901", "savings_account", "Legal Name", "987654321", "001", "invalid", 10, "IsActive must be either 'true' or 'false' as a string.")] 
    [InlineData("12345678901", "savings_account", "Legal Name", "987654321", "001", "true", -1, "Fee must be in between 0 and 100.")] 
    [InlineData("12345678901", "savings_account", "Legal Name", "987654321", "001", "true", 101, "Fee must be in between 0 and 100.")]
    public async Task Given_InvalidRequest_When_Upsert_Then_ShouldReturnBadRequest(
        string documentNumber,
        string accountType,
        string legalName,
        string bankAccount,
        string bankCode,
        string isActive,
        int fee,
        string expectedErrorMessage)
    {
        // Arrange
        var request = new UpsertCompanyRequest
        {
            AccountType = accountType,
            LegalName = legalName,
            BankAccount = bankAccount,
            BankCode = bankCode,
            IsActive = isActive,
            Fee = fee
        };

        // Act
        var response = await PutAsync($"/companies/register/{documentNumber}", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain(expectedErrorMessage);
    }

    [Fact]
    public async Task Given_ValidRequest_When_Upsert_Then_ShouldReturnCreated()
    {
        // Arrange
        var documentNumber = "12345678901";
        var request = CreateValidUpsertCompanyRequest();

        // Act
        var response = await PutAsync($"/companies/register/{documentNumber}", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}
