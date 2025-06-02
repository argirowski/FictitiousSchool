using Application.DTOs;
using MediatR;

namespace Application.Features.Commands.CreateApplication
{
    public class SubmitApplicationCommand : IRequest<Guid>
    {
        public int CourseId { get; set; }
        public Guid CourseDateId { get; set; }
        public required CompanyDTO Company { get; set; }
        public required List<ParticipantDTO> Participants { get; set; }
    }
}
