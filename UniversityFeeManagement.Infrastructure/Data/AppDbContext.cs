using Microsoft.EntityFrameworkCore;
using UniversityFeeManagement.Domain.Entities;

namespace UniversityFeeManagement.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options ) : base(options)
    {
        
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Fee> Fees { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>()
             .HasIndex(s => s.Email)
             .IsUnique();

        modelBuilder.Entity<Fee>()
             .HasOne(f => f.Student)
             .WithMany(s => s.Fees)
             .HasForeignKey(f => f.StudentId);

        modelBuilder.Entity<Fee>()
             .HasOne(f => f.Course)
             .WithMany(c => c.Fees)
             .HasForeignKey(f => f.CourseId);
    }
} 