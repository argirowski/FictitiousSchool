using Application.DTOs;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Validators
{
    public abstract class BaseValidator<T> : AbstractValidator<T>
    {
        protected readonly IFictitiousSchoolApplicationRepository _fictitiousSchoolApplicationRepository;
        protected readonly ICourseRepository _courseRepository;
        protected readonly ICourseDateRepository _courseDateRepository;
        protected readonly ICompanyRepository _companyRepository;

        protected BaseValidator(
            IFictitiousSchoolApplicationRepository fictitiousSchoolApplicationRepository,
            ICourseRepository courseRepository,
            ICourseDateRepository courseDateRepository,
            ICompanyRepository companyRepository)
        {
            _fictitiousSchoolApplicationRepository = fictitiousSchoolApplicationRepository;
            _courseRepository = courseRepository;
            _courseDateRepository = courseDateRepository;
            _companyRepository = companyRepository;
            _companyRepository = companyRepository;
        }

        protected bool BeAValidGuid(Guid id)
        {
            return id != Guid.Empty;
        }

        protected bool ApplicationExists(Guid id)
        {
            var application = _fictitiousSchoolApplicationRepository.GetByIdAsync(id).Result;
            return application != null;
        }

        protected bool CourseExists(int courseId)
        {
            var course = _courseRepository.GetByIdAsync(courseId).Result;
            return course != null;
        }

        protected bool HaveMatchingCourseId(int courseId, Guid courseDateId)
        {
            var courseDate = _courseDateRepository.GetByIdAsync(courseDateId).Result;
            return courseDate != null && courseDate.CourseId == courseId;
        }

        protected bool CompanyNameExists(string companyName)
        {
            var companies = _companyRepository.GetAllAsync().Result;
            return companies.Any(c => c.Name == companyName);
        }

        protected bool ParticipantNameExists(string participantName)
        {
            var applications = _fictitiousSchoolApplicationRepository.GetAllAsync().Result;
            return applications.SelectMany(a => a.Participants).Any(p => p.Name == participantName);
        }
    }

    public class CompanyDTOValidator : AbstractValidator<CompanyDTO>
    {
        public CompanyDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Company name is required.");

            RuleFor(x => x.Phone)
                .NotEmpty()
                .WithMessage("Company phone is required.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("A valid company email is required.");
        }
    }

    public class ParticipantDTOValidator : AbstractValidator<ParticipantDTO>
    {
        public ParticipantDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Participant name is required.");

            RuleFor(x => x.Phone)
                .NotEmpty()
                .WithMessage("Participant phone is required.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("A valid participant email is required.");
        }
    }
}
