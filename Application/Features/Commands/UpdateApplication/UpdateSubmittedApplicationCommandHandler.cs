using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Commands.UpdateApplication
{
    public class UpdateSubmittedApplicationCommandHandler : IRequestHandler<UpdateSubmittedApplicationCommand, Unit>
    {
        private readonly IFictitiousSchoolApplicationRepository _fictitiousSchoolApplicationRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public UpdateSubmittedApplicationCommandHandler(IFictitiousSchoolApplicationRepository fictitiousSchoolApplicationRepository, ICourseRepository courseRepository, ICompanyRepository companyRepository, IMapper mapper)
        {
            _fictitiousSchoolApplicationRepository = fictitiousSchoolApplicationRepository;
            _courseRepository = courseRepository;
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateSubmittedApplicationCommand request, CancellationToken cancellationToken)
        {
            var application = await _fictitiousSchoolApplicationRepository.GetByIdAsync(request.Id);
            var course = await _courseRepository.GetByIdAsync(request.CourseId);
            var company = await _companyRepository.GetByIdAsync(application.CompanyId);

            company.Name = request.Company.Name;
            company.Phone = request.Company.Phone;
            company.Email = request.Company.Email;

            application.CourseId = request.CourseId;
            application.Course = course;
            application.Company = company;
            application.Participants = _mapper.Map<List<Participant>>(request.Participants);

            await _fictitiousSchoolApplicationRepository.UpdateAsync(application);

            return Unit.Value;
        }
    }
}