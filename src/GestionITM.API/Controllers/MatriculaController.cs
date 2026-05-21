using GestionITM.Domain.Dtos;
using GestionITM.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

            var matricula = await _matriculaService.CreateAsync(matriculaDto);
            return Ok(matricula);
        }
    }
}
