using Application.DTOs;
using Application.Features.Queries.GetCourseDatesByCourseId;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace FictitiousSchoolUnitTests.HandlerTests
{
    public class GetCourseDatesByCourseIdQueryHandlerTests
    {
        private readonly Mock<ICourseDateRepository> _courseDateRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetCourseDatesByCourseIdQueryHandler _handler;

        public GetCourseDatesByCourseIdQueryHandlerTests()
        {
            _courseDateRepositoryMock = new Mock<ICourseDateRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetCourseDatesByCourseIdQueryHandler(_courseDateRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsListOfCourseDateDTOs()
        {
            // Arrange
            var courseDates = new List<CourseDate>
            {
                new CourseDate { Id = Guid.NewGuid(), Date = DateTime.Now, CourseId = 1 },
                new CourseDate { Id = Guid.NewGuid(), Date = DateTime.Now.AddDays(1), CourseId = 1 }
            };
            var courseDateDTOs = new List<CourseDateDTO>
            {
                new CourseDateDTO { Id = courseDates[0].Id, Date = courseDates[0].Date },
                new CourseDateDTO { Id = courseDates[1].Id, Date = courseDates[1].Date }
            };
            _courseDateRepositoryMock.Setup(repo => repo.GetByCourseIdAsync(It.IsAny<int>())).ReturnsAsync(courseDates);
            _mapperMock.Setup(m => m.Map<IEnumerable<CourseDateDTO>>(It.IsAny<IEnumerable<CourseDate>>())).Returns(courseDateDTOs);

            // Act
            var result = await _handler.Handle(new GetCourseDatesByCourseIdQuery(1), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<CourseDateDTO>>(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task Handle_ReturnsEmptyListOfCourseDateDTOs()
        {
            // Arrange
            var courseDates = new List<CourseDate>();
            var courseDateDTOs = new List<CourseDateDTO>();
            _courseDateRepositoryMock.Setup(repo => repo.GetByCourseIdAsync(It.IsAny<int>())).ReturnsAsync(courseDates);
            _mapperMock.Setup(m => m.Map<IEnumerable<CourseDateDTO>>(It.IsAny<IEnumerable<CourseDate>>())).Returns(courseDateDTOs);

            // Act
            var result = await _handler.Handle(new GetCourseDatesByCourseIdQuery(1), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<CourseDateDTO>>(result);
            Assert.Empty(result);
        }
    }
}