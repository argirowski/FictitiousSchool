using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class CourseDateRepository : ICourseDateRepository
    {
        private readonly ApplicationDbContext _context;

        public CourseDateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CourseDate?> GetByIdAsync(Guid courseDateId)
        {
            return await _context.CourseDates.FirstOrDefaultAsync(cd => cd.Id == courseDateId);
        }
        public async Task<IEnumerable<CourseDate>> GetByCourseIdAsync(int courseId)
        {
            return await _context.CourseDates
                .Where(cd => cd.CourseId == courseId)
                .ToListAsync();
        }
    }
}
