namespace GestionITM.Domain.Dtos;

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public int EstudianteId { get; set; }
}
