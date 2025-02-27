using Application.DTOs;
using Application.Features.Commands.CreateApplication;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation.TestHelper;
using Moq;

namespace FictitiousSchoolUnitTests.ValidatorTests
{
    public class SubmitApplicationCommandValidatorTests
    {
        private readonly Mock<IFictitiousSchoolApplicationRepository> _applicationRepositoryMock;
        private readonly Mock<ICourseRepository> _courseRepositoryMock;
        private readonly Mock<ICourseDateRepository> _courseDateRepositoryMock;
        private readonly Mock<ICompanyRepository> _companyRepositoryMock;
        private readonly SubmitApplicationCommandValidator _validator;

        public SubmitApplicationCommandValidatorTests()
        {
            _applicationRepositoryMock = new Mock<IFictitiousSchoolApplicationRepository>();
            _courseRepositoryMock = new Mock<ICourseRepository>();
            _courseDateRepositoryMock = new Mock<ICourseDateRepository>();
            _companyRepositoryMock = new Mock<ICompanyRepository>();
            _validator = new SubmitApplicationCommandValidator(
                _applicationRepositoryMock.Object,
                _courseRepositoryMock.Object,
                _courseDateRepositoryMock.Object,
                _companyRepositoryMock.Object);
        }

        [Fact]
        public async Task Validate_ShouldHaveError_WhenCourseIdIsZero()
        {
            // Arrange
            var command = new SubmitApplicationCommand
            {
                CourseId = 0,
                CourseDateId = Guid.NewGuid(),
                Company = new CompanyDTO { Name = "Company 1", Phone = "1234567890", Email = "company1@example.com" },
                Participants = new List<ParticipantDTO>
                {
                    new ParticipantDTO { Name = "Participant 1", Phone = "1234567890", Email = "participant1@example.com" }
                }
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CourseId)
                .WithErrorMessage("CourseId must be greater than 0.");
        }

        [Fact]
        public async Task Validate_ShouldHaveError_WhenCourseDoesNotExist()
        {
            // Arrange
            var command = new SubmitApplicationCommand
            {
                CourseId = 1,
                CourseDateId = Guid.NewGuid(),
                Company = new CompanyDTO { Name = "Company 1", Phone = "1234567890", Email = "company1@example.com" },
                Participants = new List<ParticipantDTO>
                {
                    new ParticipantDTO { Name = "Participant 1", Phone = "1234567890", Email = "participant1@example.com" }
                }
            };
            _courseRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Course)null);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CourseId)
                .WithErrorMessage("Course not found.");
        }

        [Fact]
        public async Task Validate_ShouldHaveError_WhenCourseDateIdIsEmpty()
        {
            // Arrange
            var command = new SubmitApplicationCommand
            {
                CourseId = 1,
                CourseDateId = Guid.Empty,
                Company = new CompanyDTO { Name = "Company 1", Phone = "1234567890", Email = "company1@example.com" },
                Participants = new List<ParticipantDTO>
                {
                    new ParticipantDTO { Name = "Participant 1", Phone = "1234567890", Email = "participant1@example.com" }
                }
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CourseDateId)
                .WithErrorMessage("CourseDateId is required.");
        }

        [Fact]
        public async Task Validate_ShouldHaveError_WhenCourseDateDoesNotMatchCourseId()
        {
            // Arrange
            var command = new SubmitApplicationCommand
            {
                CourseId = 1,
                CourseDateId = Guid.NewGuid(),
                Company = new CompanyDTO { Name = "Company 1", Phone = "1234567890", Email = "company1@example.com" },
                Participants = new List<ParticipantDTO>
                {
                    new ParticipantDTO { Name = "Participant 1", Phone = "1234567890", Email = "participant1@example.com" }
                }
            };
            var courseDate = new CourseDate { Id = command.CourseDateId, CourseId = 2 };
            _courseDateRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(courseDate);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CourseDateId)
                .WithErrorMessage("Course date not found or does not match the CourseId.");
        }

        [Fact]
        public async Task Validate_ShouldNotHaveError_WhenCommandIsValid()
        {
            // Arrange
            var command = new SubmitApplicationCommand
            {
                CourseId = 1,
                CourseDateId = Guid.NewGuid(),
                Company = new CompanyDTO { Name = "Company 2", Phone = "1234567890", Email = "company2@example.com" },
                Participants = new List<ParticipantDTO>
                {
                    new ParticipantDTO { Name = "Participant 2", Phone = "1234567890", Email = "participant2@example.com" }
                }
            };
            _courseRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Course { Id = 1 });
            _courseDateRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new CourseDate { Id = command.CourseDateId, CourseId = 1 });
            _companyRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Company>());
            _applicationRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<FictitiousSchoolApplication>());

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.CourseId);
            result.ShouldNotHaveValidationErrorFor(x => x.CourseDateId);
            result.ShouldNotHaveValidationErrorFor(x => x.Company);
            result.ShouldNotHaveValidationErrorFor(x => x.Participants[0].Name);
        }
    }
}