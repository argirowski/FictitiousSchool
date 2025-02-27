using Domain.Interfaces;
using MediatR;

namespace Application.Features.Commands.DeleteApplication
{
    public class DeleteSubmittedApplicationCommandHandler : IRequestHandler<DeleteSubmittedApplicationCommand, Unit>
    {
        private readonly IFictitiousSchoolApplicationRepository _repository;

        public DeleteSubmittedApplicationCommandHandler(IFictitiousSchoolApplicationRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteSubmittedApplicationCommand request, CancellationToken cancellationToken)
        {
            await _repository.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}
