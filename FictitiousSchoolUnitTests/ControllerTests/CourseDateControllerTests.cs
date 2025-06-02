using API.Controllers;
using Application.DTOs;
using Application.Features.Queries.GetCourseDatesByCourseId;
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
        public async Task GetCourseDatesByCourseId_ReturnsOkResult_WithCourseDates()
        {
            // Arrange
            var courseId = 1;
            var courseDates = new List<CourseDateDTO>
            {
                new CourseDateDTO { Id = Guid.NewGuid(), Date = DateTime.UtcNow },
                new CourseDateDTO { Id = Guid.NewGuid(), Date = DateTime.UtcNow.AddDays(1) }
            };
            _mediatorMock.Setup(m => m.Send(It.Is<GetCourseDatesByCourseIdQuery>(q => q.CourseId == courseId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(courseDates);

            // Act
            var result = await _controller.GetCourseDatesByCourseId(courseId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<CourseDateDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetCourseDatesByCourseId_ReturnsOkResult_WithEmptyList()
        {
            // Arrange
            var courseId = 2;
            var courseDates = new List<CourseDateDTO>();
            _mediatorMock.Setup(m => m.Send(It.Is<GetCourseDatesByCourseIdQuery>(q => q.CourseId == courseId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(courseDates);

            // Act
            var result = await _controller.GetCourseDatesByCourseId(courseId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<CourseDateDTO>>(okResult.Value);
            Assert.Empty(returnValue);
        }
    }
}