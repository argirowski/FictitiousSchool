using API.Controllers;
using Application.DTOs;
using Application.Features.Queries.GetAllCourses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace FictitiousSchoolUnitTests.ControllerTests
{
    public class CourseControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly CourseController _controller;

        public CourseControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new CourseController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetAllCourses_ReturnsOkResult_WithCourseList()
        {
            // Arrange
            var courses = new List<CourseDTO>
            {
                new CourseDTO { Id = 1, Name = "Math" },
                new CourseDTO { Id = 2, Name = "Science" }
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllCoursesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(courses);

            // Act
            var result = await _controller.GetAllCourses();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<CourseDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetAllCourses_ReturnsOkResult_WithEmptyList()
        {
            // Arrange
            var courses = new List<CourseDTO>();
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllCoursesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(courses);

            // Act
            var result = await _controller.GetAllCourses();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<CourseDTO>>(okResult.Value);
            Assert.Empty(returnValue);
        }
    }
}