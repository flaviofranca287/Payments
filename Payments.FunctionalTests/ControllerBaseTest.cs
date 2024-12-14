using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Payments.WebApi;
using Payments.WebApi.Controllers.DataContracts;
using Xunit;

namespace Payments.FunctionalTests;

public abstract class ControllerBaseTest : IClassFixture<WebApplicationFactory<Program>>
{
    protected readonly HttpClient Client;

    protected ControllerBaseTest(WebApplicationFactory<Program> factory)
    {
        Client = factory.CreateClient();
    }

    protected async Task<HttpResponseMessage> PutAsync<T>(string url, T content)
    {
        var jsonContent = JsonContent.Create(content);
        return await Client.PutAsync(url, jsonContent);
    }

    protected async Task<HttpResponseMessage> PostAsync<T>(string url, T content)
    {
        var jsonContent = JsonContent.Create(content);
        return await Client.PostAsync(url, jsonContent);
    }
    
    protected async Task<T> ReadContentAsync<T>(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();
        return System.Text.Json.JsonSerializer.Deserialize<T>(json);
    }
    
    protected static UpsertCompanyRequest CreateValidUpsertCompanyRequest()
    {
        return new UpsertCompanyRequest
        {
            AccountType = "savings_account",
            LegalName = "Valid Company Name",
            BankAccount = "987654321",
            BankCode = "001",
            IsActive = "true",
            Fee = 10m
        };
    }
    
    protected static UpsertClientRequest CreateUpsertClientRequest()
    {
        return new UpsertClientRequest
        {
            ClientName = "Test Client",
            PaymentsInfo = new PaymentsInfo
            {
                CardHolderName = "Card Holder",
                CardExpirationDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1)),
                CardNumber = "4111111111111111",
                CardVerificationValue = "123",
                PaymentMethod = "credit_card"
            }
        };
    }
}