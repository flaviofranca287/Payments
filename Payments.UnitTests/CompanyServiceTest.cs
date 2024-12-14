using FluentAssertions;
using Moq;
using Payments.Application.CompanyServices;
using Payments.Application.Dto;
using Payments.Domain;
using Payments.Domain.Repositories;
using Payments.Domain.ValueObjects;
using Xunit;

namespace Payments.UnitTests;

public class CompanyServiceTest
{
    private CompanyService _companyService = null!;
    private readonly Mock<ICompaniesRepository> _companiesRepositoryMock = new();

    [Fact]
    public async Task Given_NonExistentCompany_When_Upsert_Then_ShouldInsert()
    {
        // Arrange
        var upsertOperation = CreateUpsertCompanyOperation();
        var newCompany = upsertOperation.ToCompanies();

        _companiesRepositoryMock
            .Setup(repo => repo.GetAsync(upsertOperation.DocumentNumber))
            .ReturnsAsync((Companies)null!);

        _companiesRepositoryMock
            .Setup(repo => repo.InsertAsync(It.IsAny<Companies>()))
            .ReturnsAsync(newCompany);

        _companiesRepositoryMock
            .Setup(repo => repo.UpdateAsync(It.IsAny<Companies>()));

        _companyService = new CompanyService(_companiesRepositoryMock.Object);

        // Act
        var result = await _companyService.Upsert(upsertOperation);

        // Assert
        _companiesRepositoryMock.Verify(repo => repo.GetAsync(upsertOperation.DocumentNumber), Times.Once);
        _companiesRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<Companies>()), Times.Once);
        _companiesRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Companies>()), Times.Never);

        AssertUpsertCompanyOperation(result, upsertOperation);
    }

    [Fact]
    public async Task Given_AnExistentCompany_When_Upsert_Then_ShouldUpdate()
    {
        // Arrange
        var upsertOperation = CreateUpsertCompanyOperation();
        var existingCompany = upsertOperation.ToCompanies();

        _companiesRepositoryMock
            .Setup(repo => repo.GetAsync(upsertOperation.DocumentNumber))
            .ReturnsAsync(existingCompany);

        _companiesRepositoryMock
            .Setup(repo => repo.UpdateAsync(It.IsAny<Companies>()))
            .ReturnsAsync(existingCompany);

        _companiesRepositoryMock
            .Setup(repo => repo.InsertAsync(It.IsAny<Companies>()));

        _companyService = new CompanyService(_companiesRepositoryMock.Object);

        // Act
        var result = await _companyService.Upsert(upsertOperation);

        // Assert
        _companiesRepositoryMock.Verify(repo => repo.GetAsync(upsertOperation.DocumentNumber), Times.Once);
        _companiesRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<Companies>()), Times.Never);
        _companiesRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Companies>()), Times.Once);

        AssertUpsertCompanyOperation(result, upsertOperation);
    }

    private static UpsertCompanyOperation CreateUpsertCompanyOperation()
    {
        return new UpsertCompanyOperation
        {
            DocumentNumber = "12345678901",
            LegalName = "AnyCompany",
            AccountType = "savings_account",
            BankAccount = "987654321",
            BankCode = "001",
            IsActive = true,
            Fee = 2.5m
        };
    }

    private static void AssertUpsertCompanyOperation(UpsertCompanyOperation result, UpsertCompanyOperation upsertOperation)
    {
        result.Should().NotBeNull();
        result.DocumentNumber.Should().Be(upsertOperation.DocumentNumber);
        result.LegalName.Should().Be(upsertOperation.LegalName);
        result.AccountType.Should().Be(AccountType.SavingsAccount.ToString());
        result.BankAccount.Should().Be(upsertOperation.BankAccount);
        result.BankCode.Should().Be(upsertOperation.BankCode);
        result.IsActive.Should().Be(upsertOperation.IsActive);
        result.Fee.Should().Be(upsertOperation.Fee);
    }
}
