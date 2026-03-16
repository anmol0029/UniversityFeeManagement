using Microsoft.AspNetCore.Mvc;
using UniversityFeeManagement.Application.DTOs;
using UniversityFeeManagement.Application.Interfaces;
using UniversityFeeManagement.Domain.Entities;
using AutoMapper;

namespace UniversityFeeManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourseController : ControllerBase
{
    private readonly ICourseRepository _repo;
    private readonly IMapper _mapper;

    public CourseController(ICourseRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses()
    {
        var courses = await _repo.GetAllAsync();
        var result = _mapper.Map<IEnumerable<CourseDto>>(courses);

        return Ok(result);
    }

    
    [HttpGet("{id}")]
    public async Task<ActionResult<CourseDto>> GetCourse(int id)
    {
        var course = await _repo.GetByIdAsync(id);

        if (course == null)
            return NotFound();

        var result = _mapper.Map<CourseDto>(course);

        return Ok(result);
    }

    
    [HttpPost]
    public async Task<ActionResult<CourseDto>> Create(CourseDto dto)
    {
        var course = _mapper.Map<Course>(dto);

        await _repo.AddAsync(course);

        var result = _mapper.Map<CourseDto>(course);

        return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, result);
    }

   
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCourse(int id, CourseDto dto)
    {
        if (id != dto.Id)
            return BadRequest();

        var course = _mapper.Map<Course>(dto);

        await _repo.UpdateAsync(course);

        return NoContent();
    }

    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        await _repo.DeleteAsync(id);

        return NoContent();
    }
}