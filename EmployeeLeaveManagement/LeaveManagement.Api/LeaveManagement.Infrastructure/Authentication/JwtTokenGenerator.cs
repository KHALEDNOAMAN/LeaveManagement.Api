// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Text;
// using LeaveManagement.Application.Interfaces;
// using LeaveManagement.Domain.Entities;
// using Microsoft.Extensions.Configuration;
// using Microsoft.IdentityModel.Tokens;

// namespace LeaveManagement.Infrastructure.Authentication;

// public class JwtTokenGenerator : IJwtTokenGenerator
// {
//     private readonly IConfiguration _configuration;

//     public JwtTokenGenerator(IConfiguration configuration)
//     {
//         _configuration = configuration;
//     }

//     public (string Token, DateTime ExpiresAt) GenerateToken(User user)
//     {
//         var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
//             _configuration["JwtSettings:Secret"] ?? "SuperSecretKeyForLeaveManagementSystem2026!"));

//         var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
//         var expiresAt = DateTime.UtcNow.AddHours(8);

//         var claims = new[]
//         {
//             new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
//             new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
//             new Claim(ClaimTypes.Role, user.Role),
//             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
//         };

//         var token = new JwtSecurityToken(
//             issuer: _configuration["JwtSettings:Issuer"] ?? "LeaveManagementAPI",
//             audience: _configuration["JwtSettings:Audience"] ?? "LeaveManagementApp",
//             claims: claims,
//             expires: expiresAt,
//             signingCredentials: credentials);

//         return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
//     }
// }

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LeaveManagement.Application.Interfaces;
using LeaveManagement.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LeaveManagement.Infrastructure.Authentication;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration _configuration;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public (string Token, DateTime ExpiresAt) GenerateToken(User user)
    {
        var secret = _configuration["JwtSettings:Secret"] ?? "SuperSecretKeyForLeaveManagementSystem2026!";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiresAt = DateTime.UtcNow.AddHours(8);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"] ?? "LeaveManagementAPI",
            audience: _configuration["JwtSettings:Audience"] ?? "LeaveManagementApp",
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }
}