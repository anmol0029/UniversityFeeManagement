using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using UniversityFeeManagement.Domain.Entities;
using UniversityFeeManagement.Infrastructure.Data;
using UniversityFeeManagement.Infrastructure.Email;
using UniversityFeeManagement.Application.Interfaces;

namespace UniversityFeeManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IUserRepository _repo;
    private readonly JwtService _jwt;
    private readonly IOtpRepository _otpRepo;
    private readonly OtpService _otpService;
    private readonly EmailService _emailService;
    private readonly AuthService _authService;

    public AuthController(
        AppDbContext context,
        IUserRepository repo,
        JwtService jwt,
        IOtpRepository otpRepo,
        OtpService otpService,
        EmailService emailService,
        AuthService authService)
    {
        _context = context;
        _repo = repo;
        _jwt = jwt;
        _otpRepo = otpRepo;
        _otpService = otpService;
        _emailService = emailService;
        _authService = authService;
    }

    
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        if (await _repo.EmailExistsAsync(dto.Email))
           return BadRequest("Email already exists");

        var user = new User
        {
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = "User"
        };

        
        await _repo.AddAsync(user);

        return Ok("Registered successfully");
    }

    
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var token = await _authService.Login(dto);

        if (token == null)
           return Unauthorized("Invalid credentials");

        return Ok(new{ token });
    }

    
    [AllowAnonymous]
    [HttpPost("send-otp")]
    public async Task<IActionResult> SendOtp(SendOtpDto dto)
    {
        var user = await _repo.GetByEmailAsync(dto.Email);

        if (user == null)
            return NotFound("Email not registered");

        var rawOtp = _otpService.GenerateOtp();

        var otp = new Otp
        {
            Email = dto.Email,
            Code = BCrypt.Net.BCrypt.HashPassword(rawOtp),
            ExpiryTime = DateTime.UtcNow.AddMinutes(5),
            IsUsed = false
        };

        await _otpRepo.SaveOtpAsync(otp);

        await _emailService.SendEmailAsync(
            dto.Email,
            "Your OTP Code",
            $"Your OTP is: {rawOtp}"
        );

        return Ok("OTP sent successfully");
    }

    
    [AllowAnonymous]
    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp(VerifyOtpDto dto)
    {
        var otp = await _otpRepo.GetValidOtpAsync(dto.Email, dto.Code);

        if (otp == null)
            return BadRequest("Invalid or expired OTP");

        var user = await _repo.GetByEmailAsync(dto.Email);

        if (user == null)
            return NotFound("User not found");

        await _otpRepo.MarkAsUsedAsync(otp);

        var token = _jwt.GenerateToken(user);

        return Ok(new
        {
            token = token,
            email = user.Email
        });
    }

    [Authorize]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
     {
   
       var email = User.FindFirst(ClaimTypes.Email)?.Value;

       if (string.IsNullOrEmpty(email))
          return Unauthorized("Invalid token");

    
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

    
        await _repo.UpdatePasswordAsync(email, hashedPassword);

    
       await _emailService.SendEmailAsync(
          email,
           "Password Changed Successfully",
           "Your password has been changed successfully. If this was not you, please contact support immediately."
     );

         return Ok(new { message = "Password updated successfully" });
  }
}