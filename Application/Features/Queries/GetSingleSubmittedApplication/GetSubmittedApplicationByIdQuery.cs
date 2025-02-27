using Application.DTOs;
using MediatR;

namespace Application.Features.Queries.GetSingleSubmittedApplication
{
    public class GetSubmittedApplicationByIdQuery : IRequest<FictitiousSchoolApplicationDTO>
    {
        public Guid Id { get; set; }

        public GetSubmittedApplicationByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
