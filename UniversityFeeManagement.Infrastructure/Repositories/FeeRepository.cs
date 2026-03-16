using Microsoft.EntityFrameworkCore;
using UniversityFeeManagement.Application.Interfaces;
using UniversityFeeManagement.Domain.Entities;
using UniversityFeeManagement.Infrastructure.Data;

namespace UniversityFeeManagement.Infrastructure.Repositories;

public class FeeRepository : IFeeRepository
{
    private readonly AppDbContext _context;

    public FeeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Fee>> GetAllAsync()
    {
        return await _context.Fees
            .Include(f => f.Student)
            .Include(f => f.Course)
            .ToListAsync();
    }

    public async Task<Fee?> GetByIdAsync(int id)
    {
        return await _context.Fees
            .Include(f => f.Student)
            .Include(f => f.Course)
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task AddAsync(Fee fee)
    {
        await _context.Fees.AddAsync(fee);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Fee fee)
    {
        _context.Fees.Update(fee);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var fee = await _context.Fees.FindAsync(id);

        if (fee != null)
        {
            _context.Fees.Remove(fee);
            await _context.SaveChangesAsync();
        }
    }
}