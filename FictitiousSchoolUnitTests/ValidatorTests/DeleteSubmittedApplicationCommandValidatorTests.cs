using Application.Features.Commands.DeleteApplication;
using Application.Validators;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation.TestHelper;
using Moq;

namespace FictitiousSchoolUnitTests.ValidatorTests
{
    public class DeleteSubmittedApplicationCommandValidatorTests
    {
        private readonly Mock<IFictitiousSchoolApplicationRepository> _applicationRepositoryMock;
        private readonly Mock<ICourseRepository> _courseRepositoryMock;
        private readonly Mock<ICourseDateRepository> _courseDateRepositoryMock;
        private readonly Mock<ICompanyRepository> _companyRepositoryMock;
        private readonly DeleteSubmittedApplicationCommandValidator _validator;

        public DeleteSubmittedApplicationCommandValidatorTests()
        {
            _applicationRepositoryMock = new Mock<IFictitiousSchoolApplicationRepository>();
            _courseRepositoryMock = new Mock<ICourseRepository>();
            _courseDateRepositoryMock = new Mock<ICourseDateRepository>();
            _companyRepositoryMock = new Mock<ICompanyRepository>();
            _validator = new DeleteSubmittedApplicationCommandValidator(
                _applicationRepositoryMock.Object,
                _courseRepositoryMock.Object,
                _courseDateRepositoryMock.Object,
                _companyRepositoryMock.Object);
        }

        [Fact]
        public async Task Validate_ShouldHaveError_WhenIdIsEmpty()
        {
            // Arrange
            var command = new DeleteSubmittedApplicationCommand(Guid.Empty);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Id is required.");
        }

        [Fact]
        public async Task Validate_ShouldHaveError_WhenIdIsNotValidGuid()
        {
            // Arrange
            var command = new DeleteSubmittedApplicationCommand(Guid.Empty);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Id must be a valid GUID.");
        }

        [Fact]
        public async Task Validate_ShouldHaveError_WhenApplicationDoesNotExist()
        {
            // Arrange
            var command = new DeleteSubmittedApplicationCommand(Guid.NewGuid());
            _applicationRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((FictitiousSchoolApplication)null);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Application not found.");
        }

        [Fact]
        public async Task Validate_ShouldNotHaveError_WhenCommandIsValid()
        {
            // Arrange
            var command = new DeleteSubmittedApplicationCommand(Guid.NewGuid());
            _applicationRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new FictitiousSchoolApplication());

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Id);
        }
    }
}