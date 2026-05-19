using GestionITM.Domain.Entities;

namespace GestionITM.Domain.Interfaces
{
    public interface IProfesorRepository
    {
        // Operaciones de acceso a datos para Profesor
        Task<IEnumerable<Profesor>> ObtenerTodosAsync();
        Task AgregarAsync(Profesor profesor);

        // Nivel 5: consulta diferida para paginación y filtros (IQueryable)
        IQueryable<Profesor> ConsultarTodo();
    }
}
