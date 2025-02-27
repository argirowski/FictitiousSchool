using Application.DTOs;
using Application.Features.Queries.GetSingleSubmittedApplication;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace FictitiousSchoolUnitTests.HandlerTests
{
    public class GetSubmittedApplicationByIdQueryHandlerTests
    {
        private readonly Mock<IFictitiousSchoolApplicationRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetSubmittedApplicationByIdQueryHandler _handler;

        public GetSubmittedApplicationByIdQueryHandlerTests()
        {
            _repositoryMock = new Mock<IFictitiousSchoolApplicationRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetSubmittedApplicationByIdQueryHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsFictitiousSchoolApplicationDTO()
        {
            // Arrange
            var applicationId = Guid.NewGuid();
            var application = new FictitiousSchoolApplication
            {
                Id = applicationId,
                CourseId = 1,
                Company = new Company { Id = Guid.NewGuid(), Name = "Company 1" }
            };
            var applicationDTO = new FictitiousSchoolApplicationDTO
            {
                Id = applicationId,
                Course = new CourseDTO { Id = 1, Name = "Course 1" },
                Company = new CompanyDTO { Name = "Company 1" }
            };
            _repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(application);
            _mapperMock.Setup(m => m.Map<FictitiousSchoolApplicationDTO>(It.IsAny<FictitiousSchoolApplication>())).Returns(applicationDTO);

            // Act
            var result = await _handler.Handle(new GetSubmittedApplicationByIdQuery(applicationId), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<FictitiousSchoolApplicationDTO>(result);
            Assert.Equal(applicationId, result.Id);
        }

        [Fact]
        public async Task Handle_ReturnsNull_WhenApplicationNotFound()
        {
            // Arrange
            var applicationId = Guid.NewGuid();
            _repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((FictitiousSchoolApplication)null);

            // Act
            var result = await _handler.Handle(new GetSubmittedApplicationByIdQuery(applicationId), CancellationToken.None);

            // Assert
            Assert.Null(result);
        }
    }
}