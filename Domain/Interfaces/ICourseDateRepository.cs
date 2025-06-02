using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ICourseDateRepository
    {
        Task<CourseDate?> GetByIdAsync(Guid courseDateId);
        Task<IEnumerable<CourseDate>> GetByCourseIdAsync(int courseId);
    }
}
