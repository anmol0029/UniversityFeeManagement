using UniversityFeeManagement.Domain.Entities;

namespace UniversityFeeManagement.Application.Interfaces;

public interface IFeeRepository
{
    Task<IEnumerable<Fee>> GetAllAsync();
    Task<Fee?> GetByIdAsync(int id);
    Task AddAsync(Fee fee);
    Task UpdateAsync(Fee fee);
    Task DeleteAsync(int id);
}