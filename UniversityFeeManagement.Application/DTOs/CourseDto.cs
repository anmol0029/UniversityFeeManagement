namespace UniversityFeeManagement.Application.DTOs;

    public class CourseDto
    {
        public int Id { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public decimal CourseFee { get; set; }
    }
