using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ICourseRepository
    {
        Task<Course?> GetByIdAsync(int courseId);
        Task<IEnumerable<Course>> GetAllAsync();
    }
}
