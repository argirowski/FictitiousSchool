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
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

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
        public async Task SubmitApplication_ReturnsOkResult_WithGuid()
        {
            // Arrange
            var command = new SubmitApplicationCommand
            {
                CourseId = 1,
                CourseDateId = Guid.NewGuid(),
                Company = new CompanyDTO { Name = "Test", Phone = "123", Email = "test@test.com" },
                Participants = new List<ParticipantDTO>
                {
                    new ParticipantDTO { Name = "P1", Phone = "111", Email = "p1@test.com" }
                }
            };
            var expectedGuid = Guid.NewGuid();
            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedGuid);

            // Act
            var result = await _controller.SubmitApplication(command);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(expectedGuid, okResult.Value);
        }

        [Fact]
        public async Task GetAllSubmittedApplications_ReturnsOkResult_WithApplications()
        {
            // Arrange
            var applications = new List<FictitiousSchoolApplicationDTO>
            {
                new FictitiousSchoolApplicationDTO
                {
                    Id = Guid.NewGuid(),
                    Course = new CourseDTO { Id = 1, Name = "Math" },
                    CourseDate = new CourseDateDTO { Id = Guid.NewGuid(), Date = DateTime.UtcNow },
                    Company = new CompanyDTO { Name = "Test", Phone = "123", Email = "test@test.com" },
                    Participants = new List<ParticipantDTO>
                    {
                        new ParticipantDTO { Name = "P1", Phone = "111", Email = "p1@test.com" }
                    }
                }
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllSubmittedApplicationsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(applications);

            // Act
            var result = await _controller.GetAllSubmittedApplications();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<FictitiousSchoolApplicationDTO>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetSubmittedApplicationById_ReturnsOkResult_WithApplication()
        {
            // Arrange
            var id = Guid.NewGuid();
            var application = new FictitiousSchoolApplicationDTO
            {
                Id = id,
                Course = new CourseDTO { Id = 1, Name = "Math" },
                CourseDate = new CourseDateDTO { Id = Guid.NewGuid(), Date = DateTime.UtcNow },
                Company = new CompanyDTO { Name = "Test", Phone = "123", Email = "test@test.com" },
                Participants = new List<ParticipantDTO>
                {
                    new ParticipantDTO { Name = "P1", Phone = "111", Email = "p1@test.com" }
                }
            };
            _mediatorMock.Setup(m => m.Send(It.Is<GetSubmittedApplicationByIdQuery>(q => q.Id == id), It.IsAny<CancellationToken>()))
                .ReturnsAsync(application);

            // Act
            var result = await _controller.GetSubmittedApplicationById(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<FictitiousSchoolApplicationDTO>(okResult.Value);
            Assert.Equal(id, returnValue.Id);
        }

        [Fact]
        public async Task DeleteSubmittedApplication_ReturnsNoContent()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mediatorMock.Setup(m => m.Send(It.Is<DeleteSubmittedApplicationCommand>(c => c.Id == id), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            // Act
            var result = await _controller.DeleteSubmittedApplication(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateSubmittedApplication_ReturnsNoContent()
        {
            // Arrange
            var id = Guid.NewGuid();
            var command = new UpdateSubmittedApplicationCommand(
                id,
                1,
                Guid.NewGuid(),
                new CompanyDTO { Name = "Test", Phone = "123", Email = "test@test.com" },
                new List<ParticipantDTO>
                {
                    new ParticipantDTO { Name = "P1", Phone = "111", Email = "p1@test.com" }
                }
            );
            _mediatorMock.Setup(m => m.Send(It.Is<UpdateSubmittedApplicationCommand>(c => c.Id == id), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            // Act
            var result = await _controller.UpdateSubmittedApplication(id, command);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}