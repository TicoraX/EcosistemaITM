using GestionITM.Domain.Entities;

namespace GestionITM.Domain.Interfaces
{
    public interface ICursoRepository
    {
        Task<IEnumerable<Curso>> ObtenerTodoAsync();
        Task<Curso?> ObtenerPorIdAsync(int id);
        Task AgregarAsync(Curso curso);
        Task ActualizarAsync(Curso curso);
        Task<GestionITM.Domain.Models.PagedResult<Curso>> ObtenerPaginadoAsync(int pageNumber, int pageSize);
    }
}
