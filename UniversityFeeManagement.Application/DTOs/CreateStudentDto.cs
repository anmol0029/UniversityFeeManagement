using System.ComponentModel.DataAnnotations;

namespace UniversityFeeManagement.Application.DTOs;

    public class CreateStudentDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
