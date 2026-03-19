using Microsoft.AspNetCore.Mvc;
using UniversityFeeManagement.Application.DTOs;
using UniversityFeeManagement.Application.Interfaces;
using UniversityFeeManagement.Domain.Entities;
using AutoMapper;
using UniversityFeeManagement.Infrastructure.Email;
using Microsoft.AspNetCore.Authorization;

namespace UniversityFeeManagement.API.Controllers;
[Authorize]
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
        return Ok(_mapper.Map<IEnumerable<StudentDto>>(students));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StudentDto>> GetStudent(int id)
    {
        var student = await _repo.GetByIdAsync(id);

        if (student == null)
            return NotFound();

        return Ok(_mapper.Map<StudentDto>(student));
    }

    [HttpPost]
    public async Task<ActionResult<StudentDto>> Create(CreateStudentDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (await _repo.EmailExistsAsync(dto.Email))
            return BadRequest("Email already exists");

        var student = _mapper.Map<Student>(dto);

        await _repo.AddAsync(student);

        try
        {
            await _emailService.SendEmailAsync(
                dto.Email,
                "Welcome",
                "Your account has been created"
            );
        }
        catch
        {
            
        }

        return CreatedAtAction(nameof(GetStudent),
            new { id = student.Id },
            _mapper.Map<StudentDto>(student));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudent(int id, StudentDto dto)
    {
        if (id != dto.Id)
            return BadRequest();

        var existing = await _repo.GetByIdAsync(id);
        if (existing == null)
            return NotFound();

        await _repo.UpdateAsync(_mapper.Map<Student>(dto));

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null)
            return NotFound();

        await _repo.DeleteAsync(id);

        return NoContent();
    }
}