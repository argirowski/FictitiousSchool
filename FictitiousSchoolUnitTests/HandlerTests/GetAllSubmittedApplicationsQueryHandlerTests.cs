using Application.DTOs;
using Application.Features.Queries.GetAllSubmittedApplications;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace FictitiousSchoolUnitTests.HandlerTests
{
    public class GetAllSubmittedApplicationsQueryHandlerTests
    {
        private readonly Mock<IFictitiousSchoolApplicationRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllSubmittedApplicationsQueryHandler _handler;

        public GetAllSubmittedApplicationsQueryHandlerTests()
        {
            _repositoryMock = new Mock<IFictitiousSchoolApplicationRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetAllSubmittedApplicationsQueryHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsListOfFictitiousSchoolApplicationDTOs()
        {
            // Arrange
            var applications = new List<FictitiousSchoolApplication>
            {
                new FictitiousSchoolApplication
                {
                    Id = Guid.NewGuid(),
                    CourseId = 1,
                    Company = new Company
                    {
                        Id = Guid.NewGuid(),
                        Name = "Company 1",
                        Phone = "123-456-7890",
                        Email = "company1@example.com"
                    }
                },
                new FictitiousSchoolApplication
                {
                    Id = Guid.NewGuid(),
                    CourseId = 2,
                    Company = new Company
                    {
                        Id = Guid.NewGuid(),
                        Name = "Company 2",
                        Phone = "987-654-3210",
                        Email = "company2@example.com"
                    }
                }
            };
            var applicationDTOs = new List<FictitiousSchoolApplicationDTO>
            {
                new FictitiousSchoolApplicationDTO
                {
                    Id = applications[0].Id,
                    Course = new CourseDTO { Id = 1, Name = "Course 1" },
                    Company = new CompanyDTO { Name = "Company 1", Phone = "123-456-7890", Email = "company1@example.com" },
                    CourseDate = new CourseDateDTO { Id = Guid.NewGuid(), Date = DateTime.Now },
                    Participants = new List<ParticipantDTO>()
                },
                new FictitiousSchoolApplicationDTO
                {
                    Id = applications[1].Id,
                    Course = new CourseDTO { Id = 2, Name = "Course 2" },
                    Company = new CompanyDTO { Name = "Company 2", Phone = "987-654-3210", Email = "company2@example.com" },
                    CourseDate = new CourseDateDTO { Id = Guid.NewGuid(), Date = DateTime.Now },
                    Participants = new List<ParticipantDTO>()
                }
            };
            _repositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(applications);
            _mapperMock.Setup(m => m.Map<IEnumerable<FictitiousSchoolApplicationDTO>>(It.IsAny<IEnumerable<FictitiousSchoolApplication>>())).Returns(applicationDTOs);

            // Act
            var result = await _handler.Handle(new GetAllSubmittedApplicationsQuery(), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<FictitiousSchoolApplicationDTO>>(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task Handle_ReturnsEmptyListOfFictitiousSchoolApplicationDTOs()
        {
            // Arrange
            var applications = new List<FictitiousSchoolApplication>();
            var applicationDTOs = new List<FictitiousSchoolApplicationDTO>();
            _repositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(applications);
            _mapperMock.Setup(m => m.Map<IEnumerable<FictitiousSchoolApplicationDTO>>(It.IsAny<IEnumerable<FictitiousSchoolApplication>>())).Returns(applicationDTOs);

            // Act
            var result = await _handler.Handle(new GetAllSubmittedApplicationsQuery(), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<FictitiousSchoolApplicationDTO>>(result);
            Assert.Empty(result);
        }
    }
}