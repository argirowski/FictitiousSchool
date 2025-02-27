using Application.DTOs;
using MediatR;

namespace Application.Features.Queries.GetAllCourses
{
    public class GetAllCoursesQuery : IRequest<IEnumerable<CourseDTO>>
    {
    }
}
