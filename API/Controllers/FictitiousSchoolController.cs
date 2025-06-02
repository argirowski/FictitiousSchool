using Application.DTOs;
using Application.Features.Commands.CreateApplication;
using Application.Features.Commands.DeleteApplication;
using Application.Features.Commands.UpdateApplication;
using Application.Features.Queries.GetAllSubmittedApplications;
using Application.Features.Queries.GetSingleSubmittedApplication;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FictitiousSchoolController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FictitiousSchoolController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> SubmitApplication([FromBody] SubmitApplicationCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FictitiousSchoolApplicationDTO>>> GetAllSubmittedApplications()
        {
            var result = await _mediator.Send(new GetAllSubmittedApplicationsQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FictitiousSchoolApplicationDTO>> GetSubmittedApplicationById(Guid id)
        {
            var result = await _mediator.Send(new GetSubmittedApplicationByIdQuery(id));
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubmittedApplication(Guid id)
        {
            await _mediator.Send(new DeleteSubmittedApplicationCommand(id));
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubmittedApplication(Guid id, [FromBody] UpdateSubmittedApplicationCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
