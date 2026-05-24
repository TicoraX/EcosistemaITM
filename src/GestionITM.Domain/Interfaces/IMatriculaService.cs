using GestionITM.Domain.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionITM.Domain.Interfaces
{
    public interface IMatriculaService
    {
        Task<IEnumerable<MatriculaDto>> GetAllAsync();
        Task<MatriculaDto?> GetByIdAsync(int id);
        Task<MatriculaDto> CreateAsync(int estudianteId, MatriculaCreateDto matriculaDto);
    }
}
