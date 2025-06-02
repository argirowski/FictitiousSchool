using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IFictitiousSchoolApplicationRepository
    {
        Task AddAsync(FictitiousSchoolApplication submitApplication);
        Task<IEnumerable<FictitiousSchoolApplication>> GetAllAsync();
        Task<FictitiousSchoolApplication?> GetByIdAsync(Guid id);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(FictitiousSchoolApplication submitApplication);
    }
}
