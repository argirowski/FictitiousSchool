using API.Controllers;
using Application.DTOs;
using Application.Features.Queries.GetCourseDatesByCourseId;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FictitiousSchoolUnitTests.ControllerTests
{
    public class CourseDateControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly CourseDateController _controller;

        public CourseDateControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new CourseDateController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetCourseDatesByCourseId_ReturnsOkResult_WithListOfCourseDates()
        {
            // Arrange
            var courseDates = new List<CourseDateDTO>
            {
                new CourseDateDTO { Id = Guid.NewGuid(), Date = DateTime.Now },
                new CourseDateDTO { Id = Guid.NewGuid(), Date = DateTime.Now.AddDays(1) }
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetCourseDatesByCourseIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(courseDates);

            // Act
            var result = await _controller.GetCourseDatesByCourseId(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<CourseDateDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetCourseDatesByCourseId_ReturnsOkResult_WithEmptyList()
        {
            // Arrange
            var courseDates = new List<CourseDateDTO>();
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetCourseDatesByCourseIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(courseDates);

            // Act
            var result = await _controller.GetCourseDatesByCourseId(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<CourseDateDTO>>(okResult.Value);
            Assert.Empty(returnValue);
        }
    }
}