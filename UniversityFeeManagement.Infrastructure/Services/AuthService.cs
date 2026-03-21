using UniversityFeeManagement.Application.Interfaces;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtService _jwtService;

    public AuthService(IUserRepository userRepository, JwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<string?> Login(LoginDto dto)
   {
       var user = await _userRepository.GetByEmailAsync(dto.Email);

       if (user == null || string.IsNullOrEmpty(user.PasswordHash))
          return null;

       var isValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

       if (!isValid)
          return null;

       return _jwtService.GenerateToken(user);
    }
}