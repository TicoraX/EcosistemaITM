using GestionITM.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionITM.Domain.Interfaces
{
    public interface IMatriculaRepository
    {
        Task<IEnumerable<Matricula>> GetAllAsync();
        Task<Matricula?> GetByIdAsync(int id);
        Task<bool> ExistsAsync(int estudianteId, int cursoId);
        Task<Matricula> AddAsync(Matricula matricula);
    }
}
