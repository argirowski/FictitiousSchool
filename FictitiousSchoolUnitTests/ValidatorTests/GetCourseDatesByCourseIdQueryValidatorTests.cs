using Application.Features.Queries.GetCourseDatesByCourseId;
using Application.Validators;
using Domain.Interfaces;
using FluentValidation.TestHelper;
using Moq;

namespace FictitiousSchoolUnitTests.ValidatorTests
{
    public class GetCourseDatesByCourseIdQueryValidatorTests
    {
        private readonly Mock<IFictitiousSchoolApplicationRepository> _fictitiousSchoolApplicationRepositoryMock;
        private readonly Mock<ICourseRepository> _courseRepositoryMock;
        private readonly Mock<ICourseDateRepository> _courseDateRepositoryMock;
        private readonly Mock<ICompanyRepository> _companyRepositoryMock;
        private readonly GetCourseDatesByCourseIdQueryValidator _validator;

        public GetCourseDatesByCourseIdQueryValidatorTests()
        {
            _fictitiousSchoolApplicationRepositoryMock = new Mock<IFictitiousSchoolApplicationRepository>();
            _courseRepositoryMock = new Mock<ICourseRepository>();
            _courseDateRepositoryMock = new Mock<ICourseDateRepository>();
            _companyRepositoryMock = new Mock<ICompanyRepository>();

            _validator = new GetCourseDatesByCourseIdQueryValidator(
                _fictitiousSchoolApplicationRepositoryMock.Object,
                _courseRepositoryMock.Object,
                _courseDateRepositoryMock.Object,
                _companyRepositoryMock.Object
            );
        }

        [Fact]
        public void Should_Pass_When_Course_Exists()
        {
            // Arrange
            int courseId = 1;
            _courseRepositoryMock
                .Setup(repo => repo.GetByIdAsync(courseId))
                .ReturnsAsync(new Domain.Entities.Course { Id = courseId, Name = "Test Course" });

            var query = new GetCourseDatesByCourseIdQuery(courseId);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.CourseId);
        }

        [Fact]
        public void Should_Fail_When_Course_Does_Not_Exist()
        {
            // Arrange
            int courseId = 2;
            _courseRepositoryMock
                .Setup(repo => repo.GetByIdAsync(courseId))
                .ReturnsAsync((Domain.Entities.Course?)null);

            var query = new GetCourseDatesByCourseIdQuery(courseId);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CourseId)
                .WithErrorMessage("Course with the specified ID does not exist.");
        }
    }
}