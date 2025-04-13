using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Repositories;

namespace FictitiousSchoolUnitTests.RepositoryTests
{
    public class CompanyRepositoryTests
    {
        private DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        }

        // To ensure that each test runs in isolation, you can use a new in-memory database for each test.This can be achieved by creating
        // a new ApplicationDbContext instance for each test.

        //1.    CreateNewContextOptions: This method creates a new DbContextOptions instance with a unique in-memory database name for each test.
        //2.	Arrange: In each test, create a new ApplicationDbContext instance using the CreateNewContextOptions method.This ensures that each test runs in isolation with a fresh in-memory database.
        //3.	Act: Call the GetByIdAsync and GetAllAsync methods on the CompanyRepository.
        //4.	Assert: Verify that the result is the expected company or list of companies, or null when the company does not exist.
        //By following these steps, you ensure that each test runs in isolation with a fresh in-memory database, preventing data from previous tests from affecting the current test. This should resolve the test failure.

        [Fact]
        public async Task GetByIdAsync_ReturnsCompany_WhenCompanyExists()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new ApplicationDbContext(options);
            var repository = new CompanyRepository(context);

            var companyId = Guid.NewGuid();
            var company = new Company { Id = companyId, Name = "Company 1", Phone = "1234567890", Email = "company1@example.com" };
            context.Companies.Add(company);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetByIdAsync(companyId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(companyId, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenCompanyDoesNotExist()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new ApplicationDbContext(options);
            var repository = new CompanyRepository(context);

            var companyId = Guid.NewGuid();

            // Act
            var result = await repository.GetByIdAsync(companyId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsListOfCompanies()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new ApplicationDbContext(options);
            var repository = new CompanyRepository(context);

            var companies = new List<Company>
            {
                new Company { Id = Guid.NewGuid(), Name = "Company 1", Phone = "1234567890", Email = "company1@example.com" },
                new Company { Id = Guid.NewGuid(), Name = "Company 2", Phone = "0987654321", Email = "company2@example.com" }
            };
            context.Companies.AddRange(companies);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
    }
}