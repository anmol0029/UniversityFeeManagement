using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UniversityFeeManagement.Domain.Entities;

public class JwtService
{
    private readonly IConfiguration _config;

    public JwtService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(User user)
   {

      var keyValue = _config["Jwt:Key"];

      if (string.IsNullOrEmpty(keyValue))
         throw new Exception("JWT Key missing");

        var claims = new[]
     {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role)
     };
      
       var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyValue));
       var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    

       var token = new JwtSecurityToken(
           issuer: _config["Jwt:Issuer"],
           audience: _config["Jwt:Audience"],
           claims: claims,
           expires: DateTime.Now.AddHours(2),
           signingCredentials: creds
    );

         return new JwtSecurityTokenHandler().WriteToken(token);
   }

}