using Application.DTOs;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Queries.GetCourseDatesByCourseId
{
    public class GetCourseDatesByCourseIdQueryHandler : IRequestHandler<GetCourseDatesByCourseIdQuery, IEnumerable<CourseDateDTO>>
    {
        private readonly ICourseDateRepository _courseDateRepository;
        private readonly IMapper _mapper;

        public GetCourseDatesByCourseIdQueryHandler(ICourseDateRepository courseDateRepository, IMapper mapper)
        {
            _courseDateRepository = courseDateRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CourseDateDTO>> Handle(GetCourseDatesByCourseIdQuery request, CancellationToken cancellationToken)
        {
            var courseDates = await _courseDateRepository.GetByCourseIdAsync(request.CourseId);
            return _mapper.Map<IEnumerable<CourseDateDTO>>(courseDates);
        }
    }
}
