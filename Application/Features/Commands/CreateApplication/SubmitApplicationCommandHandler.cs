using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Commands.CreateApplication
{
    public class SubmitApplicationCommandHandler : IRequestHandler<SubmitApplicationCommand, Guid>
    {
        private readonly IFictitiousSchoolApplicationRepository _fictitiousSchoolApplicationRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseDateRepository _courseDateRepository;
        private readonly IMapper _mapper;

        public SubmitApplicationCommandHandler(IFictitiousSchoolApplicationRepository fictitiousSchoolApplicationRepository, ICourseRepository courseRepository, ICourseDateRepository courseDateRepository, IMapper mapper)
        {
            _fictitiousSchoolApplicationRepository = fictitiousSchoolApplicationRepository;
            _courseRepository = courseRepository;
            _courseDateRepository = courseDateRepository;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(SubmitApplicationCommand request, CancellationToken cancellationToken)
        {
            var course = await _courseRepository.GetByIdAsync(request.CourseId);

            var courseDate = await _courseDateRepository.GetByIdAsync(request.CourseDateId);

            var company = _mapper.Map<Company>(request.Company);

            var application = new FictitiousSchoolApplication
            {
                Id = Guid.NewGuid(),
                CourseId = request.CourseId,
                CourseDateId = request.CourseDateId,
                CompanyId = Guid.NewGuid(),
                Course = course,
                CourseDate = courseDate,
                Company = company,
                Participants = _mapper.Map<List<Participant>>(request.Participants)
            };

            await _fictitiousSchoolApplicationRepository.AddAsync(application);

            return application.Id;
        }
    }
}