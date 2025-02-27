using Application.DTOs;
using Application.Features.Queries.GetAllCourses;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace FictitiousSchoolUnitTests.HandlerTests
{
    public class GetAllCoursesQueryHandlerTests
    {
        private readonly Mock<ICourseRepository> _courseRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllCoursesQueryHandler _handler;

        public GetAllCoursesQueryHandlerTests()
        {
            _courseRepositoryMock = new Mock<ICourseRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetAllCoursesQueryHandler(_courseRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsListOfCourseDTOs()
        {
            // Arrange
            var courses = new List<Course>
            {
                new Course { Id = 1, Name = "Course 1" },
                new Course { Id = 2, Name = "Course 2" }
            };
            var courseDTOs = new List<CourseDTO>
            {
                new CourseDTO { Id = 1, Name = "Course 1" },
                new CourseDTO { Id = 2, Name = "Course 2" }
            };
            _courseRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(courses);
            _mapperMock.Setup(m => m.Map<IEnumerable<CourseDTO>>(It.IsAny<IEnumerable<Course>>())).Returns(courseDTOs);

            // Act
            var result = await _handler.Handle(new GetAllCoursesQuery(), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<CourseDTO>>(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task Handle_ReturnsEmptyListOfCourseDTOs()
        {
            // Arrange
            var courses = new List<Course>();
            var courseDTOs = new List<CourseDTO>();
            _courseRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(courses);
            _mapperMock.Setup(m => m.Map<IEnumerable<CourseDTO>>(It.IsAny<IEnumerable<Course>>())).Returns(courseDTOs);

            // Act
            var result = await _handler.Handle(new GetAllCoursesQuery(), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<CourseDTO>>(result);
            Assert.Empty(result);
        }
    }
}