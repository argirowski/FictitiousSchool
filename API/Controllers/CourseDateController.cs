using Application.Features.Queries.GetCourseDatesByCourseId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseDateController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CourseDateController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetCourseDatesByCourseId(int courseId)
        {
            var result = await _mediator.Send(new GetCourseDatesByCourseIdQuery(courseId));
            return Ok(result);
        }
    }
}
