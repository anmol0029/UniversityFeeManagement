using Microsoft.EntityFrameworkCore;
using UniversityFeeManagement.Application.Interfaces;
using UniversityFeeManagement.Domain.Entities;
using UniversityFeeManagement.Infrastructure.Data;

namespace UniversityFeeManagement.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
          return await _context.Users.AnyAsync(x => x.Email == email);
    }

    public async Task UpdatePasswordAsync(string email, string newPasswordHash)
   {
       var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

         if (user != null)
         {
             user.PasswordHash = newPasswordHash;
             await _context.SaveChangesAsync();
          }
    }
}