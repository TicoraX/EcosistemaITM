using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GestionITM.Domain.Dtos;
using GestionITM.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GestionITM.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IEstudianteRepository _estudianteRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IEstudianteRepository estudianteRepository, IConfiguration configuration)
    {
        _estudianteRepository = estudianteRepository;
        _configuration = configuration;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
    {
        var estudiante = await _estudianteRepository.ObtenerPorCorreoAsync(request.Email.Trim());
        if (estudiante == null || string.IsNullOrEmpty(estudiante.PasswordHash))
            return null;

        if (!BCrypt.Net.BCrypt.Verify(request.Password, estudiante.PasswordHash))
            return null;

        var token = GenerateJwtToken(estudiante.Id, estudiante.Correo);
        return new LoginResponseDto
        {
            Token = token,
            EstudianteId = estudiante.Id
        };
    }

    private string GenerateJwtToken(int estudianteId, string correo)
    {
        var jwtKey = _configuration["Jwt:Key"]
            ?? Environment.GetEnvironmentVariable("JWT_KEY")
            ?? throw new InvalidOperationException("Jwt:Key o JWT_KEY debe estar configurado.");

        var issuer = _configuration["Jwt:Issuer"] ?? "GestionITMAPI";
        var audience = _configuration["Jwt:Audience"] ?? "EstudiantesITM";

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, estudianteId.ToString()),
            new Claim(ClaimTypes.Email, correo),
            new Claim(ClaimTypes.Role, "Estudiante")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
