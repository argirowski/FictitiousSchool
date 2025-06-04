using Application.Features.Queries.GetCourseDatesByCourseId;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Validators
{
    public class GetCourseDatesByCourseIdQueryValidator : BaseValidator<GetCourseDatesByCourseIdQuery>
    {
        public GetCourseDatesByCourseIdQueryValidator(
            IFictitiousSchoolApplicationRepository fictitiousSchoolApplicationRepository,
            ICourseRepository courseRepository,
            ICourseDateRepository courseDateRepository,
            ICompanyRepository companyRepository)
            : base(fictitiousSchoolApplicationRepository, courseRepository, courseDateRepository, companyRepository)
        {
            RuleFor(x => x.CourseId)
                .Must(CourseExists)
                .WithMessage("Course with the specified ID does not exist.");
        }
    }
}