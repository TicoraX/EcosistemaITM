using GestionITM.Domain.Entities;
using GestionITM.Domain.Interfaces;
using GestionITM.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace GestionITM.Infrastructure.Repositories
{
    public class CursoRepository : ICursoRepository
    {
        private readonly ApplicationDbContext _context;

        public CursoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Curso>> ObtenerTodoAsync()
        {
            return await _context.Cursos.ToListAsync();
        }

        public async Task<Curso?> ObtenerPorIdAsync(int id)
        {
            return await _context.Cursos.FindAsync(id);
        }

        public async Task AgregarAsync(Curso curso)
        {
            await _context.Cursos.AddAsync(curso);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Curso curso)
        {
            _context.Cursos.Update(curso);
            await _context.SaveChangesAsync();
        }

        public async Task<GestionITM.Domain.Models.PagedResult<Curso>> ObtenerPaginadoAsync(int pageNumber, int pageSize)
        {
            var totalRecords = await _context.Cursos.CountAsync();
            var cursos = await _context.Cursos
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new GestionITM.Domain.Models.PagedResult<Curso>
            {
                Items = cursos,
                TotalRegistros = totalRecords,
                PaginaActual = pageNumber,
                RegistrosPorPagina = pageSize,
                TotalPaginas = (int)Math.Ceiling(totalRecords / (double)pageSize)
            };
        }
    }
}
