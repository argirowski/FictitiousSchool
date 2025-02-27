using Application.Features.Commands.UpdateApplication;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Validators
{
    public class UpdateSubmittedApplicationCommandValidator : BaseValidator<UpdateSubmittedApplicationCommand>
    {
        public UpdateSubmittedApplicationCommandValidator(
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

            RuleFor(x => x.CourseId)
                .GreaterThan(0)
                .WithMessage("CourseId must be greater than 0.")
                .Must(CourseExists)
                .WithMessage("Course not found.");

            RuleFor(x => x.CourseDateId)
                .NotEmpty()
                .WithMessage("CourseDateId is required.")
                .Must((command, courseDateId) => HaveMatchingCourseId(command.CourseId, courseDateId))
                .WithMessage("Course date not found or does not match the CourseId.");

            RuleFor(x => x.Company)
                .NotNull()
                .WithMessage("Company information is required.")
                .SetValidator(new CompanyDTOValidator())
                .Must(company => !CompanyNameExists(company.Name))
                .WithMessage("Company name already exists.");

            RuleForEach(x => x.Participants)
                .SetValidator(new ParticipantDTOValidator())
                .Must(participant => !ParticipantNameExists(participant.Name))
                .WithMessage("Participant name already exists.");
        }
    }
}
