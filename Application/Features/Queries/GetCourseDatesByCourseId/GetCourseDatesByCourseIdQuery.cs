using Application.DTOs;
using MediatR;

namespace Application.Features.Queries.GetCourseDatesByCourseId
{
    public class GetCourseDatesByCourseIdQuery : IRequest<IEnumerable<CourseDateDTO>>
    {
        public int CourseId { get; set; }

        public GetCourseDatesByCourseIdQuery(int courseId)
        {
            CourseId = courseId;
        }
    }
}
