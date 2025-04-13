using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Repositories;

namespace FictitiousSchoolUnitTests.RepositoryTests
{
    public class FictitiousSchoolApplicationRepositoryTests
    {
        private DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task AddAsync_AddsApplication()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new ApplicationDbContext(options);
            var repository = new FictitiousSchoolApplicationRepository(context);

            var application = new FictitiousSchoolApplication
            {
                Id = Guid.NewGuid(),
                CourseId = 1,
                CourseDateId = Guid.NewGuid(),
                CompanyId = Guid.NewGuid(),
                Company = new Company { Id = Guid.NewGuid(), Name = "Company 1", Phone = "1234567890", Email = "company1@example.com" },
                Participants = new List<Participant>
                {
                    new Participant { Id = Guid.NewGuid(), Name = "Participant 1", Phone = "1234567890", Email = "participant1@example.com" }
                }
            };

            // Act
            await repository.AddAsync(application);
            var result = await context.SubmitApplications.FindAsync(application.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(application.Id, result.Id);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsListOfApplications()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new ApplicationDbContext(options);
            var repository = new FictitiousSchoolApplicationRepository(context);

            var applications = new List<FictitiousSchoolApplication>
            {
                new FictitiousSchoolApplication
                {
                    Id = Guid.NewGuid(),
                    CourseId = 1,
                    CourseDateId = Guid.NewGuid(),
                    CompanyId = Guid.NewGuid(),
                    Course = new Course { Id = 1, Name = "Course 1" },
                    CourseDate = new CourseDate { Id = Guid.NewGuid(), Date = DateTime.Now },
                    Company = new Company { Id = Guid.NewGuid(), Name = "Company 1", Phone = "1234567890", Email = "company1@example.com" },
                    Participants = new List<Participant>
                    {
                        new Participant { Id = Guid.NewGuid(), Name = "Participant 1", Phone = "1234567890", Email = "participant1@example.com" }
                    }
                },
                new FictitiousSchoolApplication
                {
                    Id = Guid.NewGuid(),
                    CourseId = 2,
                    CourseDateId = Guid.NewGuid(),
                    CompanyId = Guid.NewGuid(),
                    Course = new Course { Id = 2, Name = "Course 2" },
                    CourseDate = new CourseDate { Id = Guid.NewGuid(), Date = DateTime.Now.AddDays(1) },
                    Company = new Company { Id = Guid.NewGuid(), Name = "Company 2", Phone = "0987654321", Email = "company2@example.com" },
                    Participants = new List<Participant>
                    {
                        new Participant { Id = Guid.NewGuid(), Name = "Participant 2", Phone = "0987654321", Email = "participant2@example.com" }
                    }
                }
            };
            context.SubmitApplications.AddRange(applications);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsApplication_WhenApplicationExists()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new ApplicationDbContext(options);
            var repository = new FictitiousSchoolApplicationRepository(context);

            var applicationId = Guid.NewGuid();
            var application = new FictitiousSchoolApplication
            {
                Id = applicationId,
                CourseId = 1,
                CourseDateId = Guid.NewGuid(),
                CompanyId = Guid.NewGuid(),
                Course = new Course { Id = 1, Name = "Course 1" },
                CourseDate = new CourseDate { Id = Guid.NewGuid(), Date = DateTime.Now },
                Company = new Company { Id = Guid.NewGuid(), Name = "Company 1", Phone = "1234567890", Email = "company1@example.com" },
                Participants = new List<Participant>
                {
                    new Participant { Id = Guid.NewGuid(), Name = "Participant 1", Phone = "1234567890", Email = "participant1@example.com" }
                }
            };
            context.SubmitApplications.Add(application);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetByIdAsync(applicationId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(applicationId, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenApplicationDoesNotExist()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new ApplicationDbContext(options);
            var repository = new FictitiousSchoolApplicationRepository(context);

            var applicationId = Guid.NewGuid();

            // Act
            var result = await repository.GetByIdAsync(applicationId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_DeletesApplication()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new ApplicationDbContext(options);
            var repository = new FictitiousSchoolApplicationRepository(context);

            var applicationId = Guid.NewGuid();
            var application = new FictitiousSchoolApplication
            {
                Id = applicationId,
                CourseId = 1,
                CourseDateId = Guid.NewGuid(),
                CompanyId = Guid.NewGuid(),
                Course = new Course { Id = 1, Name = "Course 1" },
                CourseDate = new CourseDate { Id = Guid.NewGuid(), Date = DateTime.Now },
                Company = new Company { Id = Guid.NewGuid(), Name = "Company 1", Phone = "1234567890", Email = "company1@example.com" },
                Participants = new List<Participant>
                {
                    new Participant { Id = Guid.NewGuid(), Name = "Participant 1", Phone = "1234567890", Email = "participant1@example.com" }
                }
            };
            context.SubmitApplications.Add(application);
            await context.SaveChangesAsync();

            // Act
            await repository.DeleteAsync(applicationId);
            var result = await context.SubmitApplications.FindAsync(applicationId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesApplication()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new ApplicationDbContext(options);
            var repository = new FictitiousSchoolApplicationRepository(context);

            var applicationId = Guid.NewGuid();
            var application = new FictitiousSchoolApplication
            {
                Id = applicationId,
                CourseId = 1,
                CourseDateId = Guid.NewGuid(),
                CompanyId = Guid.NewGuid(),
                Course = new Course { Id = 1, Name = "Course 1" },
                CourseDate = new CourseDate { Id = Guid.NewGuid(), Date = DateTime.Now },
                Company = new Company { Id = Guid.NewGuid(), Name = "Company 1", Phone = "1234567890", Email = "company1@example.com" },
                Participants = new List<Participant>
                {
                    new Participant { Id = Guid.NewGuid(), Name = "Participant 1", Phone = "1234567890", Email = "participant1@example.com" }
                }
            };
            context.SubmitApplications.Add(application);
            await context.SaveChangesAsync();

            // Add the new Course entity to the database context
            var newCourse = new Course { Id = 2, Name = "Course 2" };
            context.Courses.Add(newCourse);
            await context.SaveChangesAsync();

            application.CourseId = 2;
            application.Course = newCourse;

            // Act
            await repository.UpdateAsync(application);
            var result = await context.SubmitApplications.FindAsync(applicationId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.CourseId);
        }
    }
}