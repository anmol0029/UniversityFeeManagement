using Microsoft.AspNetCore.Mvc;
using UniversityFeeManagement.Application.DTOs;
using UniversityFeeManagement.Application.Interfaces;
using UniversityFeeManagement.Domain.Entities;
using AutoMapper;

namespace UniversityFeeManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeeController : ControllerBase
{
    private readonly IFeeRepository _repo;
    private readonly IMapper _mapper;

    public FeeController(IFeeRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

   
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FeeDto>>> GetFees()
    {
        var fees = await _repo.GetAllAsync();
        var result = _mapper.Map<IEnumerable<FeeDto>>(fees);

        return Ok(result);
    }

    
    [HttpGet("{id}")]
    public async Task<ActionResult<FeeDto>> GetFee(int id)
    {
        var fee = await _repo.GetByIdAsync(id);

        if (fee == null)
            return NotFound();

        var result = _mapper.Map<FeeDto>(fee);

        return Ok(result);
    }

    
    [HttpPost]
    public async Task<ActionResult<FeeDto>> Create(FeeDto dto)
    {
        var fee = _mapper.Map<Fee>(dto);

        await _repo.AddAsync(fee);

        var result = _mapper.Map<FeeDto>(fee);

        return CreatedAtAction(nameof(GetFee), new { id = fee.Id }, result);
    }

    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFee(int id, FeeDto dto)
    {
        if (id != dto.Id)
            return BadRequest();

        var fee = _mapper.Map<Fee>(dto);

        await _repo.UpdateAsync(fee);

        return NoContent();
    }

    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFee(int id)
    {
        await _repo.DeleteAsync(id);

        return NoContent();
    }
}