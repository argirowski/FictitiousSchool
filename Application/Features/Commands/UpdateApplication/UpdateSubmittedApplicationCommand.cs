using Application.DTOs;
using MediatR;

namespace Application.Features.Commands.UpdateApplication
{
    public class UpdateSubmittedApplicationCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public int CourseId { get; set; }
        public Guid CourseDateId { get; set; }
        public CompanyDTO Company { get; set; }
        public List<ParticipantDTO> Participants { get; set; }

        public UpdateSubmittedApplicationCommand(Guid id, int courseId, Guid courseDateId, CompanyDTO company, List<ParticipantDTO> participants)
        {
            Id = id;
            CourseId = courseId;
            CourseDateId = courseDateId;
            Company = company;
            Participants = participants;
        }
    }
}
