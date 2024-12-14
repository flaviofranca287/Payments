using Dapper;
using FluentAssertions;
using Payments.Domain;
using Payments.Domain.Repositories;
using Payments.Domain.ValueObjects;
using Payments.Infrastructure.Repositories;

namespace Payments.IntegrationTests;

public class CompaniesRepositoryTest : BaseRepositoryTest
{
    private readonly ICompaniesRepository _companiesRepository;
    private readonly Companies _companies;

    public CompaniesRepositoryTest()
    {
        _companiesRepository = new CompaniesRepository(DatabaseContext);
    }
    
    [Fact]
    public async Task Given_ACompany_When_GetAsync_Then_ShouldReturnAsExpected()
    {
        // Arrange
        var databaseCompany = CreateCompany();
        CreateCompanyInDatabase(databaseCompany);

        // Act
        var company = await _companiesRepository.GetAsync(databaseCompany.DocumentNumber);

        // Assert
        AssertCompany(company, databaseCompany);
    }
    
    [Fact]
    public async Task Given_ACompany_When_InsertAsync_Then_ShouldBehaveAsExpected()
    {
        // Arrange
        var toBeInsertedCompany = CreateCompany();

        // Act
        var insertedCompany = await _companiesRepository.InsertAsync(toBeInsertedCompany);

        // Assert
        var fetchedCompany = await _companiesRepository.GetAsync(toBeInsertedCompany.DocumentNumber);

        AssertCompany(fetchedCompany, toBeInsertedCompany);
        insertedCompany.Should().BeEquivalentTo(toBeInsertedCompany);

        DropCompaniesTable();
    }
    
    [Fact]
    public async Task Given_ACompany_When_UpdateAsync_Then_ShouldUpdateCorrectly()
    {
        // Arrange
        var existingCompany = CreateCompany();
        CreateCompanyInDatabase(existingCompany);

        // Update details
        existingCompany.AccountType = AccountType.SavingsAccount;
        existingCompany.IsActive = false;
        existingCompany.BankAccount = "123654321";
        existingCompany.BankCode = "321";
        existingCompany.Fee = 3.55m;
        existingCompany.LegalName = "Another name";

        // Act
        await _companiesRepository.UpdateAsync(existingCompany);

        // Assert
        var updatedCompany = await _companiesRepository.GetAsync(existingCompany.DocumentNumber);
        AssertCompany(updatedCompany, existingCompany);
    }
    
    [Fact]
    public async Task Given_ACompanyDocumentNumber_When_GetFeeAndIdAsync_Then_ShouldReturnCorrectValues()
    {
        // Arrange
        var company = CreateCompany();
        CreateCompanyInDatabase(company);

        // Act
        var (fee, id) = await _companiesRepository.GetFeeAndIdAsync(company.DocumentNumber);

        // Assert
        fee.Should().Be(company.Fee);
        id.Should().BeGreaterThan(0);
    }
    
    private static void AssertCompany(Companies companyToBeAsserted, Companies expectedCompany)
    {
        companyToBeAsserted.DocumentNumber.Should().Be(expectedCompany.DocumentNumber);
        companyToBeAsserted.AccountType.Should().Be(expectedCompany.AccountType);
        companyToBeAsserted.LegalName.Should().Be(expectedCompany.LegalName);
        companyToBeAsserted.BankAccount.Should().Be(expectedCompany.BankAccount);
        companyToBeAsserted.BankCode.Should().Be(expectedCompany.BankCode);
        companyToBeAsserted.IsActive.Should().Be(expectedCompany.IsActive);
        companyToBeAsserted.Fee.Should().Be(expectedCompany.Fee);
    }

    private static Companies CreateCompany()
    {
        return new Companies
        {
            DocumentNumber = "12345678901",
            AccountType = AccountType.CheckingAccount,
            LegalName = "Example Company",
            BankAccount = "987654321",
            BankCode = "001",
            IsActive = true,
            Fee = 2.5m
        };
    }

    private void CreateCompanyInDatabase(Companies company)
    {
        DataFactory.CreateOne("Companies", x =>
        {
            x.WithValue("DocumentNumber", company.DocumentNumber);
            x.WithValue("AccountType", company.AccountType);
            x.WithValue("LegalName", company.LegalName);
            x.WithValue("BankAccount", company.BankAccount);
            x.WithValue("BankCode", company.BankCode);
            x.WithValue("IsActive", company.IsActive);
            x.WithValue("Fee", company.Fee);
        });
    }

    private async void DropCompaniesTable()
    {
        var query = "DELETE FROM Companies";
        await DatabaseContext.Connection.ExecuteAsync(query);
    }
}
