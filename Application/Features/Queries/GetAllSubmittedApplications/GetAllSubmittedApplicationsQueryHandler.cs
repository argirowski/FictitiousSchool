using Application.DTOs;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Queries.GetAllSubmittedApplications
{
    public class GetAllSubmittedApplicationsQueryHandler(IFictitiousSchoolApplicationRepository repository, IMapper mapper) : IRequestHandler<GetAllSubmittedApplicationsQuery, IEnumerable<FictitiousSchoolApplicationDTO>>
    {
        private readonly IFictitiousSchoolApplicationRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<FictitiousSchoolApplicationDTO>> Handle(GetAllSubmittedApplicationsQuery request, CancellationToken cancellationToken)
        {
            var applications = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<FictitiousSchoolApplicationDTO>>(applications);
        }
    }
}
