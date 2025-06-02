using API.Controllers;
using Application.DTOs;
using Application.Features.Commands.CreateApplication;
using Application.Features.Commands.DeleteApplication;
using Application.Features.Commands.UpdateApplication;
using Application.Features.Queries.GetAllSubmittedApplications;
using Application.Features.Queries.GetSingleSubmittedApplication;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FictitiousSchoolUnitTests.ControllerTests
{
    public class FictitiousSchoolControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly FictitiousSchoolController _controller;

        public FictitiousSchoolControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new FictitiousSchoolController(_mediatorMock.Object);
        }

        [Fact]
        public async Task SubmitApplication_ReturnsOkResult_WithNewApplicationId()
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
            var newApplicationId = Guid.NewGuid();
            _mediatorMock.Setup(m => m.Send(It.IsAny<SubmitApplicationCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(newApplicationId);

            // Act
            var result = await _controller.SubmitApplication(command);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Guid>(okResult.Value);
            Assert.Equal(newApplicationId, returnValue);
        }

        [Fact]
        public async Task GetAllSubmittedApplications_ReturnsOkResult_WithListOfApplications()
        {
            // Arrange
            var applications = new List<FictitiousSchoolApplicationDTO>
            {
                new FictitiousSchoolApplicationDTO
                {
                    Id = Guid.NewGuid(),
                    Course = new CourseDTO { Id = 1, Name = "Course 1" },
                    CourseDate = new CourseDateDTO { Id = Guid.NewGuid(), Date = DateTime.UtcNow },
                    Company = new CompanyDTO { Name = "Company 1", Phone = "1234567890", Email = "company1@example.com" },
                    Participants = new List<ParticipantDTO>
                    {
                        new ParticipantDTO { Name = "Participant 1", Phone = "1234567890", Email = "participant1@example.com" }
                    }
                },
                new FictitiousSchoolApplicationDTO
                {
                    Id = Guid.NewGuid(),
                    Course = new CourseDTO { Id = 2, Name = "Course 2" },
                    CourseDate = new CourseDateDTO { Id = Guid.NewGuid(), Date = DateTime.UtcNow },
                    Company = new CompanyDTO { Name = "Company 2", Phone = "0987654321", Email = "company2@example.com" },
                    Participants = new List<ParticipantDTO>
                    {
                        new ParticipantDTO { Name = "Participant 2", Phone = "0987654321", Email = "participant2@example.com" }
                    }
                }
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllSubmittedApplicationsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(applications);

            // Act
            var result = await _controller.GetAllSubmittedApplications();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<FictitiousSchoolApplicationDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetAllSubmittedApplications_ReturnsOkResult_WithEmptyList()
        {
            // Arrange
            var applications = new List<FictitiousSchoolApplicationDTO>();
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllSubmittedApplicationsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(applications);

            // Act
            var result = await _controller.GetAllSubmittedApplications();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<FictitiousSchoolApplicationDTO>>(okResult.Value);
            Assert.Empty(returnValue);
        }

        [Fact]
        public async Task GetSubmittedApplicationById_ReturnsOkResult_WithApplication()
        {
            // Arrange
            var applicationId = Guid.NewGuid();
            var application = new FictitiousSchoolApplicationDTO
            {
                Id = applicationId,
                Course = new CourseDTO { Id = 1, Name = "Course 1" },
                CourseDate = new CourseDateDTO { Id = Guid.NewGuid(), Date = DateTime.UtcNow },
                Company = new CompanyDTO { Name = "Company 1", Phone = "1234567890", Email = "company1@example.com" },
                Participants = new List<ParticipantDTO>
                {
                    new ParticipantDTO { Name = "Participant 1", Phone = "1234567890", Email = "participant1@example.com" }
                }
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetSubmittedApplicationByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(application);

            // Act
            var result = await _controller.GetSubmittedApplicationById(applicationId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<FictitiousSchoolApplicationDTO>(okResult.Value);
            Assert.Equal(applicationId, returnValue.Id);
        }

        [Fact]
        public async Task DeleteSubmittedApplication_ReturnsNoContentResult()
        {
            // Arrange
            var applicationId = Guid.NewGuid();
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteSubmittedApplicationCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            // Act
            var result = await _controller.DeleteSubmittedApplication(applicationId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateSubmittedApplication_ReturnsNoContentResult()
        {
            // Arrange
            var applicationId = Guid.NewGuid();
            var command = new UpdateSubmittedApplicationCommand(applicationId, 1, Guid.NewGuid(), new CompanyDTO { Name = "Company 1", Phone = "1234567890", Email = "company1@example.com" }, new List<ParticipantDTO>
            {
                new ParticipantDTO { Name = "Participant 1", Phone = "1234567890", Email = "participant1@example.com" }
            });
            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateSubmittedApplicationCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            // Act
            var result = await _controller.UpdateSubmittedApplication(applicationId, command);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}