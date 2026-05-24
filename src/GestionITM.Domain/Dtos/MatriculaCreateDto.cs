using System.ComponentModel.DataAnnotations;

namespace GestionITM.Domain.Dtos
{
    public class MatriculaCreateDto
    {
        [Required(ErrorMessage = "El ID del Curso es requerido.")]
        public int CursoId { get; set; }

        [Required(ErrorMessage = "El Periodo es requerido.")]
        [MaxLength(20)]
        public string Periodo { get; set; } = string.Empty;
    }
}
