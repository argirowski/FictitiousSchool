using Domain.Entities;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FictitiousSchoolUnitTests.RepositoryTests
{
    public class CourseRepositoryTests
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
        public async Task GetByIdAsync_ReturnsCourse_WhenCourseExists()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new ApplicationDbContext(options);
            var repository = new CourseRepository(context);

            var courseId = 1;
            var course = new Course { Id = courseId, Name = "Course 1" };
            context.Courses.Add(course);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetByIdAsync(courseId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(courseId, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenCourseDoesNotExist()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new ApplicationDbContext(options);
            var repository = new CourseRepository(context);

            var courseId = 1;

            // Act
            var result = await repository.GetByIdAsync(courseId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsListOfCourses()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new ApplicationDbContext(options);
            var repository = new CourseRepository(context);

            var courses = new List<Course>
            {
                new Course { Id = 1, Name = "Course 1" },
                new Course { Id = 2, Name = "Course 2" }
            };
            context.Courses.AddRange(courses);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
    }
}