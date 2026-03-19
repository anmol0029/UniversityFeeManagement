using UniversityFeeManagement.Domain.Entities;

namespace UniversityFeeManagement.Application.Interfaces;

public interface IStudentRepository
{
    Task<IEnumerable<Student>> GetAllAsync();
    Task<Student?> GetByIdAsync(int id);
    Task AddAsync(Student student);
    Task UpdateAsync(Student student);
    Task DeleteAsync (int id);
    Task<Student?> GetByEmailAsync(string email);
    Task<bool> EmailExistsAsync (string email); 
}