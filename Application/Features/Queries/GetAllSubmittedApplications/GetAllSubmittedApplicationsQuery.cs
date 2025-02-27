using Application.DTOs;
using MediatR;

namespace Application.Features.Queries.GetAllSubmittedApplications
{
    public class GetAllSubmittedApplicationsQuery : IRequest<IEnumerable<FictitiousSchoolApplicationDTO>>
    {
    }
}
