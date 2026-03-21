using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using UniversityFeeManagement.Infrastructure.Data;

public class OtpRepository : IOtpRepository
{
    private readonly AppDbContext _context;

    public OtpRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task SaveOtpAsync(Otp otp)
    {
        _context.Otps.Add(otp);
        await _context.SaveChangesAsync();
    }

    public async Task<Otp?> GetValidOtpAsync(string email, string code)
   {
      return await _context.Otps
          .Where(x => x.Email == email
                 && x.Code == code
                 && !x.IsUsed
                 && x.ExpiryTime > DateTime.UtcNow)
         .OrderByDescending(x => x.Id)
         .FirstOrDefaultAsync();
    }

    public async Task MarkAsUsedAsync(Otp otp)
    {
        otp.IsUsed = true;
        await _context.SaveChangesAsync();
    }
}