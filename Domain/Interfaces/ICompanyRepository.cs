using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ICompanyRepository
    {
        Task<Company?> GetByIdAsync(Guid companyId);
        Task<IEnumerable<Company>> GetAllAsync();
    }
}
