using GestionITM.Domain.Entities;
using GestionITM.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GestionITM.Infrastructure.Repositories
{
    public class ProfesorRepository : IProfesorRepository
    {
        private readonly ApplicationDbContext _context;

        public ProfesorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Profesor>> ObtenerTodosAsync()
        {
            return await _context.Profesores.ToListAsync();
        }

        public async Task AgregarAsync(Profesor profesor)
        {
            await _context.Profesores.AddAsync(profesor);
            await _context.SaveChangesAsync();
        }

        // Nivel 5: devolvemos IQueryable para permitir filtros y paginación eficientes en SQL Server
        public IQueryable<Profesor> ConsultarTodo()
        {
            return _context.Profesores.AsQueryable();
        }
    }
}
