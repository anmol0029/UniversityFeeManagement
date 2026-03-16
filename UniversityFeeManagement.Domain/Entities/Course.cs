using System.ComponentModel.DataAnnotations;

namespace UniversityFeeManagement.Domain.Entities;

public class Course
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string CourseName { get; set; }

    [Range(0,1000000)]
    public decimal CourseFee { get; set; }
    public ICollection<Fee> Fees { get; set; }
}