using Application.DTOs;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Queries.GetSingleSubmittedApplication
{
    public class GetSubmittedApplicationByIdQueryHandler : IRequestHandler<GetSubmittedApplicationByIdQuery, FictitiousSchoolApplicationDTO>
    {
        private readonly IFictitiousSchoolApplicationRepository _repository;
        private readonly IMapper _mapper;

        public GetSubmittedApplicationByIdQueryHandler(IFictitiousSchoolApplicationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<FictitiousSchoolApplicationDTO> Handle(GetSubmittedApplicationByIdQuery request, CancellationToken cancellationToken)
        {
            var application = await _repository.GetByIdAsync(request.Id);
            return _mapper.Map<FictitiousSchoolApplicationDTO>(application);
        }
    }
}
