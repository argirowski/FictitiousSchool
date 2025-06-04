using Application.Validators;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Xunit;

namespace FictitiousSchoolUnitTests.ValidatorTests
{
    public class ValidationBehaviourTests
    {
        public class DummyRequest { }
        public class DummyResponse { }

        [Fact]
        public async Task Handle_NoValidators_CallsNext()
        {
            // Arrange
            var behaviour = new ValidationBehaviour<DummyRequest, DummyResponse>(Enumerable.Empty<IValidator<DummyRequest>>());
            var request = new DummyRequest();
            var response = new DummyResponse();
            var nextCalled = false;

            Task<DummyResponse> Next() { nextCalled = true; return Task.FromResult(response); }

            // Act
            var result = await behaviour.Handle(request, Next, default);

            // Assert
            Assert.True(nextCalled);
            Assert.Equal(response, result);
        }

        [Fact]
        public async Task Handle_ValidatorsWithNoErrors_CallsNext()
        {
            // Arrange
            var validatorMock = new Mock<IValidator<DummyRequest>>();
            validatorMock
                .Setup(v => v.Validate(It.IsAny<ValidationContext<DummyRequest>>()))
                .Returns(new ValidationResult());

            var behaviour = new ValidationBehaviour<DummyRequest, DummyResponse>(new[] { validatorMock.Object });
            var request = new DummyRequest();
            var response = new DummyResponse();
            var nextCalled = false;

            Task<DummyResponse> Next() { nextCalled = true; return Task.FromResult(response); }

            // Act
            var result = await behaviour.Handle(request, Next, default);

            // Assert
            Assert.True(nextCalled);
            Assert.Equal(response, result);
        }

        [Fact]
        public async Task Handle_ValidatorsWithErrors_ThrowsValidationException()
        {
            // Arrange
            var failures = new[] { new ValidationFailure("Property", "Error message") };
            var validatorMock = new Mock<IValidator<DummyRequest>>();
            validatorMock
                .Setup(v => v.Validate(It.IsAny<ValidationContext<DummyRequest>>()))
                .Returns(new ValidationResult(failures));

            var behaviour = new ValidationBehaviour<DummyRequest, DummyResponse>(new[] { validatorMock.Object });
            var request = new DummyRequest();

            Task<DummyResponse> Next() => Task.FromResult(new DummyResponse());

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ValidationException>(() => behaviour.Handle(request, Next, default));
            Assert.Contains("Error message", ex.Message);
        }
    }
}