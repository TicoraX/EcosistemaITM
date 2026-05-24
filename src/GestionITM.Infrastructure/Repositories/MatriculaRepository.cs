using GestionITM.Domain.Entities;
using GestionITM.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionITM.Infrastructure.Repositories
{
    public class MatriculaRepository : IMatriculaRepository
    {
        private readonly ApplicationDbContext _context;

        public MatriculaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Matricula>> GetAllAsync()
        {
            return await _context.Matriculas.ToListAsync();
        }

        public async Task<Matricula?> GetByIdAsync(int id)
        {
            return await _context.Matriculas.FindAsync(id);
        }

        public async Task<bool> ExistsAsync(int estudianteId, int cursoId)
        {
            return await _context.Matriculas.AnyAsync(m => m.EstudianteId == estudianteId && m.CursoId == cursoId);
        }

        public async Task<Matricula> AddAsync(Matricula matricula)
        {
            await _context.Matriculas.AddAsync(matricula);
            await _context.SaveChangesAsync();
            return matricula;
        }
    }
}
