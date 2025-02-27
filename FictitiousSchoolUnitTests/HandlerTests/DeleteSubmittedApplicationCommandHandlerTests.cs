using Application.Features.Commands.DeleteApplication;
using Domain.Interfaces;
using MediatR;
using Moq;

namespace FictitiousSchoolUnitTests.HandlerTests
{
    public class DeleteSubmittedApplicationCommandHandlerTests
    {
        private readonly Mock<IFictitiousSchoolApplicationRepository> _repositoryMock;
        private readonly DeleteSubmittedApplicationCommandHandler _handler;

        public DeleteSubmittedApplicationCommandHandlerTests()
        {
            _repositoryMock = new Mock<IFictitiousSchoolApplicationRepository>();
            _handler = new DeleteSubmittedApplicationCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_DeletesApplication_ReturnsUnit()
        {
            // Arrange
            var applicationId = Guid.NewGuid();
            var command = new DeleteSubmittedApplicationCommand(applicationId);

            _repositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);
            _repositoryMock.Verify(repo => repo.DeleteAsync(applicationId), Times.Once);
        }

        [Fact]
        public async Task Handle_ThrowsException_WhenApplicationNotFound()
        {
            // Arrange
            var applicationId = Guid.NewGuid();
            var command = new DeleteSubmittedApplicationCommand(applicationId);

            _repositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<Guid>())).ThrowsAsync(new KeyNotFoundException());

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}