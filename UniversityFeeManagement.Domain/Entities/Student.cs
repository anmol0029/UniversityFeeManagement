using System.ComponentModel.DataAnnotations;

namespace UniversityFeeManagement.Domain.Entities;

public class Student
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    public ICollection<Fee>? Fees { get; set; }
}