using GestionITM.Domain.Dtos;

namespace GestionITM.Domain.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
}
