using Application.Features.Queries.GetSingleSubmittedApplication;
using FluentValidation;
using Domain.Interfaces;

namespace Application.Validators
{
    public class GetSubmittedApplicationByIdQueryValidator : BaseValidator<GetSubmittedApplicationByIdQuery>
    {
        public GetSubmittedApplicationByIdQueryValidator(
            IFictitiousSchoolApplicationRepository fictitiousSchoolApplicationRepository,
            ICourseRepository courseRepository,
            ICourseDateRepository courseDateRepository,
            ICompanyRepository companyRepository)
            : base(fictitiousSchoolApplicationRepository, courseRepository, courseDateRepository, companyRepository)
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id is required.")
                .Must(BeAValidGuid)
                .WithMessage("Id must be a valid GUID.")
                .Must(ApplicationExists)
                .WithMessage("Application not found.");
        }
    }
}