using Application.Features.Commands.DeleteApplication;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Validators
{
    public class DeleteSubmittedApplicationCommandValidator : BaseValidator<DeleteSubmittedApplicationCommand>
    {
        public DeleteSubmittedApplicationCommandValidator(
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
