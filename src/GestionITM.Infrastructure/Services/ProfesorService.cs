using AutoMapper;
using GestionITM.Domain.Dtos;
using GestionITM.Domain.Entities;
using GestionITM.Domain.Interfaces;
using GestionITM.Domain.Models; // Para el PagedResult
using Microsoft.EntityFrameworkCore;
using Microsoft .Extensions.Logging;
using System.Runtime.CompilerServices; // Para ILogger en el futuro si se desea agregar logging específico en el servicio


namespace GestionITM.Infrastructure.Services
{
    public class ProfesorService : IProfesorService
    {
        private readonly IProfesorRepository _repository;
        private readonly ILogger<ProfesorService> _logger; // Para logging específico del servicio
        private readonly IMapper _mapper;

        public ProfesorService(IProfesorRepository repository, IMapper mapper, ILogger<ProfesorService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        
        }

        // 1. OBTENER TODO (Sin paginación - para uso administrativo interno)
        public async Task<IEnumerable<ProfesorDto>> ObtenerTodosAsync()
        {
            var profesores = await _repository.ObtenerTodosAsync();
            return _mapper.Map<IEnumerable<ProfesorDto>>(profesores);
        }

        // 2. Obtener el paginado y filtrado (Nivel 5: IQueryable + Skip/Take en SQL Server)
        public async Task<PagedResult<ProfesorDto>> ObtenerPaginadosAsync(ProfesorFilterDto filtro)
        {
            // FASE A : Preparar la consulta (IQueryable) con los filtros aplicados.
            // Todavía no se ha ejecutado nada en SQL; solo construimos la expresión.
            var consulta = _repository.ConsultarTodo();

            // FASE B: Aplicar filtros dinámicos
            if (!string.IsNullOrEmpty(filtro.BusquedaNombre))
            {
                consulta = consulta.Where(p => p.Nombre.Contains(filtro.BusquedaNombre));
            }

            if (!string.IsNullOrEmpty(filtro.Especialidad))
            {
                consulta = consulta.Where(p => p.Especialidad == filtro.Especialidad);
            }

            // FASE C: Conteo total de registros para paginación (se traduce a COUNT(*) en SQL)
            var totalRegistros = await consulta.CountAsync();

            // FASE D: Aplicar paginación (Skip/Take) - sólo traemos lo que cabe en la página actual
            var items = await consulta
                .Skip((filtro.Pagina - 1) * filtro.RegistrosPorPagina)
                .Take(filtro.RegistrosPorPagina)
                .ToListAsync();

            // FASE E: Empaquetado final en un PagedResult
            return new PagedResult<ProfesorDto>
            {
                Items = _mapper.Map<List<ProfesorDto>>(items),
                TotalRegistros = totalRegistros,
                PaginaActual = filtro.Pagina,
                RegistrosPorPagina = filtro.RegistrosPorPagina,
                TotalPaginas = (int)Math.Ceiling(totalRegistros / (double)filtro.RegistrosPorPagina)
            };
        }

        // 3. Registrar profesor (reglas de negocio)
        public async Task<bool> RegistrarProfesorAsync(ProfesorCreateDto profesorCreateDto)
        {
            // LOG ESTRUCTURADO: Loguear el intento de registro con detalles relevantes (sin exponer datos sensibles)
            _logger.LogInformation("Iniciando registro de profesor. Email proporcionado: {EmailProfesor}", profesorCreateDto.Email);

         

            // Regla de negocio: la Especialidad no puede ser vacía
            if (string.IsNullOrWhiteSpace(profesorCreateDto.Especialidad))
            {
                // Nivel Warning para reglas de negocio no cumplidas
                _logger.LogWarning("Intento de registro fallido. Especialidad vacía para {NombreProfesor}", profesorCreateDto.Nombre);
                throw new Exception("Especialidad vacía");
            }

            // Regla de negocio adicional: si la especialidad es "Arquitectura",
            // se imprime un log en consola indicando perfil senior.
            if (profesorCreateDto.Especialidad.Equals("Arquitectura", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Perfil Senior Detectado");
            }

            // Reto de robustez: lanzar una excepción controlada cuando el nombre sea "Error"
            if (profesorCreateDto.Nombre.Equals("Error", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("Error de prueba");
            }

            try
            {
                var profesor = _mapper.Map<Profesor>(profesorCreateDto);
                profesor.FechaContratacion = DateTime.UtcNow;

                await _repository.AgregarAsync(profesor);

                _logger.LogInformation("Profesor {NombreProfesor} registrado exitosamente.", profesorCreateDto.Nombre);
                return true;
            }
            catch (Exception ex)
            {
                // Pasamos la excepción completa como primer parámetro para guardar el StackTrace
                _logger.LogError(ex, "Error crítico de base de datos al guardar a {NombreProfesor}.", profesorCreateDto.Nombre);
                throw; 
            }
        }
    }
}
