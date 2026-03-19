using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using UniversityFeeManagement.Domain.Entities;
using UniversityFeeManagement.Infrastructure.Data;

namespace UniversityFeeManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly JwtService _jwt;

    public AuthController(AppDbContext context, JwtService jwt)
    {
        _context = context;
        _jwt = jwt;
    }

    
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        // check if email exists
        if (await _context.Users.AnyAsync(x => x.Email == dto.Email))
            return BadRequest("Email already exists");

        var user = new User
        {
            Email = dto.Email,
            Password = dto.Password // ⚠️ will improve later
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("Registered successfully");
    }

    
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Email == dto.Email && x.Password == dto.Password);

        if (user == null)
            return Unauthorized("Invalid email or password");

        var token = _jwt.GenerateToken(user.Email);

        return Ok(new
        {
            token = token
        });
    }
}