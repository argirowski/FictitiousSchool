using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class FictitiousSchoolApplicationRepository : IFictitiousSchoolApplicationRepository
    {
        private readonly ApplicationDbContext _context;

        public FictitiousSchoolApplicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(FictitiousSchoolApplication submitApplication)
        {
            await _context.SubmitApplications.AddAsync(submitApplication);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<FictitiousSchoolApplication>> GetAllAsync()
        {
            return await _context.SubmitApplications
                .Include(sa => sa.Course)
                .Include(sa => sa.CourseDate)
                .Include(sa => sa.Company)
                .Include(sa => sa.Participants)
                .OrderBy(sa => sa.Course.Name)
                .ToListAsync();
        }

        public async Task<FictitiousSchoolApplication> GetByIdAsync(Guid id)
        {
            return await _context.SubmitApplications
                .Include(sa => sa.Course)
                .Include(sa => sa.CourseDate)
                .Include(sa => sa.Company)
                .Include(sa => sa.Participants)
                .FirstOrDefaultAsync(sa => sa.Id == id);
        }

        public async Task DeleteAsync(Guid id)
        {
            var application = await _context.SubmitApplications
                .Include(sa => sa.Company)
                .Include(sa => sa.Participants)
                .FirstOrDefaultAsync(sa => sa.Id == id);

            _context.Companies.Remove(application.Company);
            _context.Participants.RemoveRange(application.Participants);
            _context.SubmitApplications.Remove(application);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(FictitiousSchoolApplication submitApplication)
        {
            _context.SubmitApplications.Update(submitApplication);
            await _context.SaveChangesAsync();
        }
    }
}
