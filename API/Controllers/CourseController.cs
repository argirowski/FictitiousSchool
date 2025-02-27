using Application.Features.Queries.GetAllCourses;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CourseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            var result = await _mediator.Send(new GetAllCoursesQuery());
            return Ok(result);
        }
    }
}
