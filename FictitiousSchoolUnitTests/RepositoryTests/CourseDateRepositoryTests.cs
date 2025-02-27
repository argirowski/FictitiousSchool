using Domain.Entities;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FictitiousSchoolUnitTests.RepositoryTests
{
    public class CourseDateRepositoryTests
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
        public async Task GetByIdAsync_ReturnsCourseDate_WhenCourseDateExists()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new ApplicationDbContext(options);
            var repository = new CourseDateRepository(context);

            var courseDateId = Guid.NewGuid();
            var courseDate = new CourseDate { Id = courseDateId, Date = DateTime.Now, CourseId = 1 };
            context.CourseDates.Add(courseDate);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetByIdAsync(courseDateId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(courseDateId, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenCourseDateDoesNotExist()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new ApplicationDbContext(options);
            var repository = new CourseDateRepository(context);

            var courseDateId = Guid.NewGuid();

            // Act
            var result = await repository.GetByIdAsync(courseDateId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByCourseIdAsync_ReturnsListOfCourseDates()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new ApplicationDbContext(options);
            var repository = new CourseDateRepository(context);

            var courseDates = new List<CourseDate>
            {
                new CourseDate { Id = Guid.NewGuid(), Date = DateTime.Now, CourseId = 1 },
                new CourseDate { Id = Guid.NewGuid(), Date = DateTime.Now.AddDays(1), CourseId = 1 }
            };
            context.CourseDates.AddRange(courseDates);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetByCourseIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByCourseIdAsync_ReturnsEmptyList_WhenNoCourseDatesExistForCourseId()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new ApplicationDbContext(options);
            var repository = new CourseDateRepository(context);

            // Act
            var result = await repository.GetByCourseIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}