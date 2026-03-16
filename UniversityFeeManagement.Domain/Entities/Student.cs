using System.ComponentModel.DataAnnotations;

namespace UniversityFeeManagement.Domain.Entities;

public class Student
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [EmailAddress]
    public string Email { get; set; }
    public ICollection<Fee> Fees { get; set; }
}