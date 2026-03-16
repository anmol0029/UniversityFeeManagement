using Microsoft.AspNetCore.Mvc;
using UniversityFeeManagement.Application.DTOs;
using UniversityFeeManagement.Application.Interfaces;
using UniversityFeeManagement.Domain.Entities;
using AutoMapper;

namespace UniversityFeeManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly IStudentRepository _repo;
    private readonly IMapper _mapper;

    public StudentController(IStudentRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
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