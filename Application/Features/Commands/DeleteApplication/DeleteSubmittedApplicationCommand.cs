using MediatR;

namespace Application.Features.Commands.DeleteApplication
{
    public class DeleteSubmittedApplicationCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }

        public DeleteSubmittedApplicationCommand(Guid id)
        {
            Id = id;
        }
    }
}
