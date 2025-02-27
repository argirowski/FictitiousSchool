using Application.DTOs;
using Application.Features.Commands.UpdateApplication;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Moq;

namespace FictitiousSchoolUnitTests.HandlerTests
{
    public class UpdateSubmittedApplicationCommandHandlerTests
    {
        private readonly Mock<IFictitiousSchoolApplicationRepository> _applicationRepositoryMock;
        private readonly Mock<ICourseRepository> _courseRepositoryMock;
        private readonly Mock<ICompanyRepository> _companyRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateSubmittedApplicationCommandHandler _handler;

        public UpdateSubmittedApplicationCommandHandlerTests()
        {
            _applicationRepositoryMock = new Mock<IFictitiousSchoolApplicationRepository>();
            _courseRepositoryMock = new Mock<ICourseRepository>();
            _companyRepositoryMock = new Mock<ICompanyRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new UpdateSubmittedApplicationCommandHandler(
                _applicationRepositoryMock.Object,
                _courseRepositoryMock.Object,
                _companyRepositoryMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_UpdatesApplication_ReturnsUnit()
        {
            // Arrange
            var applicationId = Guid.NewGuid();
            var command = new UpdateSubmittedApplicationCommand(
                applicationId,
                1,
                Guid.NewGuid(),
                new CompanyDTO { Name = "Company 1", Phone = "1234567890", Email = "company1@example.com" },
                new List<ParticipantDTO>
                {
                    new ParticipantDTO { Name = "Participant 1", Phone = "1234567890", Email = "participant1@example.com" }
                });

            var application = new FictitiousSchoolApplication
            {
                Id = applicationId,
                CourseId = 1,
                Company = new Company { Id = Guid.NewGuid(), Name = "Company 1", Phone = "1234567890", Email = "company1@example.com" }
            };
            var course = new Course { Id = 1, Name = "Course 1" };
            var company = new Company { Id = application.Company.Id, Name = "Company 1", Phone = "1234567890", Email = "company1@example.com" };

            _applicationRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(application);
            _courseRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(course);
            _companyRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(company);
            _mapperMock.Setup(m => m.Map<List<Participant>>(It.IsAny<List<ParticipantDTO>>())).Returns(new List<Participant>());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);
            _applicationRepositoryMock.Verify(repo => repo.UpdateAsync(application), Times.Once);
        }
    }
}