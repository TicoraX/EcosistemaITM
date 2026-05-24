using GestionITM.Domain.Dtos;
using GestionITM.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GestionITM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatriculaController : ControllerBase
    {
        private readonly IMatriculaService _matriculaService;

        public MatriculaController(IMatriculaService matriculaService)
        {
            _matriculaService = matriculaService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var matriculas = await _matriculaService.GetAllAsync();
            return Ok(matriculas);
        }

        [HttpPost]
        [Authorize(Roles = "Estudiante")]
        public async Task<IActionResult> Post([FromBody] MatriculaCreateDto matriculaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var estudianteIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(estudianteIdClaim, out var estudianteId))
            {
                return Unauthorized(new { message = "No se pudo identificar el estudiante autenticado." });
            }

            var matricula = await _matriculaService.CreateAsync(estudianteId, matriculaDto);
            return Ok(matricula);
        }
    }
}
