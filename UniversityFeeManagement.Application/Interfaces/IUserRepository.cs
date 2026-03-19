using UniversityFeeManagement.Domain.Entities;

namespace UniversityFeeManagement.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task AddAsync(User user);
}