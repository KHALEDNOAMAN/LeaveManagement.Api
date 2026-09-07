using LeaveManagement.Application.DTOs;
using LeaveManagement.Infrastructure.Data;
using LeaveManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace LeaveManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _config;

    public AuthController(ApplicationDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        if (await _context.Employees.AnyAsync(e => e.Email == dto.Email))
            return BadRequest("Email is already registered.");

        // Create Employee entity
        var employee = new Employee
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Department = dto.Department,
            // In production, hash the password using BCrypt or ASP.NET PasswordHasher
        };

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Registration successful! Please log in." });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.Email == dto.Email);

        if (employee == null)
            return Unauthorized("Invalid email or password.");

        // Generate JWT Token
        var token = GenerateJwtToken(employee);

        return Ok(new AuthResponseDto(token, employee.Email, "Employee", employee.Id));
    }

    private string GenerateJwtToken(Employee employee)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
            new Claim(ClaimTypes.Email, employee.Email),
            new Claim(ClaimTypes.Role, "Employee")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? "SuperSecretKey12345678901234567890"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "LeaveApi",
            audience: "LeaveAngular",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}