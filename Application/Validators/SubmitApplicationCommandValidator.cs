using FluentValidation;
using Domain.Interfaces;
using Application.Validators;

namespace Application.Features.Commands.CreateApplication
{
    public class SubmitApplicationCommandValidator : BaseValidator<SubmitApplicationCommand>
    {
        public SubmitApplicationCommandValidator(
            IFictitiousSchoolApplicationRepository fictitiousSchoolApplicationRepository,
            ICourseRepository courseRepository,
            ICourseDateRepository courseDateRepository,
            ICompanyRepository companyRepository)
            : base(fictitiousSchoolApplicationRepository, courseRepository, courseDateRepository, companyRepository)
        {
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