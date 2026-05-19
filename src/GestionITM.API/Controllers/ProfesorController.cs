using GestionITM.Domain.Dtos;
using GestionITM.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionITM.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfesorController : ControllerBase
    {
        private readonly IProfesorService _service;

        public ProfesorController(IProfesorService service)
        {
            _service = service;
        }

        // GET: api/profesor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProfesorDto>>> Get()
        {
            var profesores = await _service.ObtenerTodosAsync();
            return Ok(profesores);
        }

        // GET: api/profesor/paginado 
        // Recibimos los datos por URL (query string) para facilitar la paginación y filtros
        [HttpGet("paginado")]
        public async Task<ActionResult> GetPaginado([FromQuery] ProfesorFilterDto filtro)
        {
            //El framework intenta llenar el objeto filtro con los datos que vienen en la query string. Si no se envía algo, se usan los valores por defecto definidos en el DTO.
            var resultado = await _service.ObtenerPaginadosAsync(filtro);
            return Ok(resultado);
        }

        /// <summary>
        /// Registra un nuevo profesor al sistema del ITM
        /// </summary>
        /// <remarks>
        /// Ejemplo de petición:
        /// 
        /// POST /api/profesor
        /// {
        /// "nombre": "Juan Pérez",
        /// "email": "juan.perez@itm.edu.do"
        /// "especialidad": "Matemáticas"
        /// }
        /// 
        /// <\remarks>
        /// <param name="profesorCreateDto">Objeto con los datos del profesor a registrar</param>
        /// <response code="200">Profesor registrado con éxito</response>
        /// <response code="400">Error al registrar el profesor (ej. especialidad vacía)</response>
        /// <response code="401">No autorizado (falta token o token inválido)</response>
      
        // POST: api/profesor
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Post([FromBody] ProfesorCreateDto profesorCreateDto)
        {
            var resultado = await _service.RegistrarProfesorAsync(profesorCreateDto);

            if (!resultado)
            {
                return BadRequest("No se pudo registrar el profesor. Verifique que la especialidad no sea vacía.");
            }

            return Ok(new { message = "Profesor registrado con éxito en el sistema del ITM." });
        }
    }
}
