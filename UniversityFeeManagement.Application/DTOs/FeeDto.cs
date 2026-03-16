namespace UniversityFeeManagement.Application.DTOs;

    public class FeeDto
    {
        public int Id { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime PaymentDate { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
    }
