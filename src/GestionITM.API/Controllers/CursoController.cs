using Microsoft.AspNetCore.Mvc;
using GestionITM.Domain.Interfaces;
using GestionITM.Domain.Entities;

namespace GestionITM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursoController : ControllerBase
    {
        private readonly ICursoRepository _repository;

        public CursoController(ICursoRepository repository)
        {
            _repository = repository;
        }

        // GET: api/curso
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Curso>>> GetCursos()
        {
            var cursos = await _repository.ObtenerTodoAsync();
            return Ok(cursos);
        }

        // GET: api/curso/paginado
        [HttpGet("paginado")]
        public async Task<ActionResult<GestionITM.Domain.Models.PagedResult<Curso>>> GetCursosPaginados([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 50) pageSize = 50;

            var result = await _repository.ObtenerPaginadoAsync(pageNumber, pageSize);
            return Ok(result);
        }

        // GET: api/curso/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Curso>> GetCurso(int id)
        {
            var curso = await _repository.ObtenerPorIdAsync(id);
            if (curso == null)
            {
                return NotFound(new { message = $"Curso con ID {id} no encontrado." });
            }
            return Ok(curso);
        }

        // POST: api/curso
        [HttpPost]
        public async Task<ActionResult> PostCurso(Curso curso)
        {
            await _repository.AgregarAsync(curso);
            return CreatedAtAction(nameof(GetCurso), new { id = curso.Id }, curso);
        }
    }
}
