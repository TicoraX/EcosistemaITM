using GestionITM.Domain.Dtos;
using GestionITM.Domain.Models;

namespace GestionITM.Domain.Interfaces
{
    public interface IProfesorService
    {
        // Operaciones de negocio para el ciclo de vida de Profesor
        Task<IEnumerable<ProfesorDto>> ObtenerTodosAsync();
        Task<bool> RegistrarProfesorAsync(ProfesorCreateDto profesorCreateDto);

        // Nivel 5: operación optimizada con IQueryable + paginación
        Task<PagedResult<ProfesorDto>> ObtenerPaginadosAsync(ProfesorFilterDto filtro);
    }
}
