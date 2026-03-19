using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityFeeManagement.Domain.Entities;

public class Fee
{
    [Key]
    public int Id { get; set; }
    public decimal AmountPaid { get; set; }
    public DateTime PaymentDate { get; set; }

    [ForeignKey("Student")]
    public int StudentId { get; set; }
    public Student? Student { get; set; }

    [ForeignKey("Course")]
    public int CourseId { get; set; }
    public Course? Course { get; set; }
}