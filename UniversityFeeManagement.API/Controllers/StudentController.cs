using Microsoft.AspNetCore.Mvc;
using UniversityFeeManagement.Application.DTOs;
using UniversityFeeManagement.Application.Interfaces;
using UniversityFeeManagement.Domain.Entities;
using AutoMapper;
using UniversityFeeManagement.Infrastructure.Email;

namespace UniversityFeeManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly IStudentRepository _repo;
    private readonly IMapper _mapper;
    private readonly EmailService _emailService;

    public StudentController(IStudentRepository repo, IMapper mapper, EmailService emailService)
    {
        _repo = repo;
        _mapper = mapper;
        _emailService = emailService;
    }

    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents()
    {
        var students = await _repo.GetAllAsync();
        var result = _mapper.Map<IEnumerable<StudentDto>>(students);

        return Ok(result);
    }

    
    [HttpGet("{id}")]
    public async Task<ActionResult<StudentDto>> GetStudent(int id)
    {
        var student = await _repo.GetByIdAsync(id);

        if (student == null)
            return NotFound();

        var result = _mapper.Map<StudentDto>(student);

        return Ok(result);
    }

    
    [HttpPost]
    public async Task<ActionResult<StudentDto>> Create(CreateStudentDto dto)
    {
        var student = _mapper.Map<Student>(dto);

        await _repo.AddAsync(student);

        await _emailService.SendEmailAsync(
            dto.Email,
            "Welcome",
            "Your account has been created"
        );

        var result = _mapper.Map<StudentDto>(student);

        return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, result);
    }

    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudent(int id, StudentDto dto)
    {
        if (id != dto.Id)
            return BadRequest();

        var student = _mapper.Map<Student>(dto);

        await _repo.UpdateAsync(student);

        return NoContent();
    }

    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        await _repo.DeleteAsync(id);

        return NoContent();
    }
}