using Application.DTOs;
using Application.Features.Commands.CreateApplication;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace FictitiousSchoolUnitTests.HandlerTests
{
    public class SubmitApplicationCommandHandlerTests
    {
        private readonly Mock<IFictitiousSchoolApplicationRepository> _applicationRepositoryMock;
        private readonly Mock<ICourseRepository> _courseRepositoryMock;
        private readonly Mock<ICourseDateRepository> _courseDateRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly SubmitApplicationCommandHandler _handler;

        public SubmitApplicationCommandHandlerTests()
        {
            _applicationRepositoryMock = new Mock<IFictitiousSchoolApplicationRepository>();
            _courseRepositoryMock = new Mock<ICourseRepository>();
            _courseDateRepositoryMock = new Mock<ICourseDateRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new SubmitApplicationCommandHandler(
                _applicationRepositoryMock.Object,
                _courseRepositoryMock.Object,
                _courseDateRepositoryMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsNewApplicationId()
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
            var course = new Course { Id = 1, Name = "Course 1" };
            var courseDate = new CourseDate { Id = command.CourseDateId, Date = DateTime.Now, CourseId = 1 };
            var company = new Company { Id = Guid.NewGuid(), Name = "Company 1", Phone = "1234567890", Email = "company1@example.com" };
            var applicationId = Guid.NewGuid();

            _courseRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(course);
            _courseDateRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(courseDate);
            _mapperMock.Setup(m => m.Map<Company>(It.IsAny<CompanyDTO>())).Returns(company);
            _mapperMock.Setup(m => m.Map<List<Participant>>(It.IsAny<List<ParticipantDTO>>())).Returns(new List<Participant>());
            _applicationRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<FictitiousSchoolApplication>())).Returns(Task.CompletedTask)
                .Callback<FictitiousSchoolApplication>(app => app.Id = applicationId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(applicationId, result);
        }
    }
}