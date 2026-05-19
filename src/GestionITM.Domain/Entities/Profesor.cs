using System.ComponentModel.DataAnnotations;

namespace GestionITM.Domain.Entities
{
    public class Profesor
    {
        // EF Core detecta "Id" como clave primaria por convención
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Especialidad { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(200)]
        public string Email { get; set; } = string.Empty;

        // Fecha en la que el profesor fue contratado por el ITM
        public DateTime FechaContratacion { get; set; } = DateTime.Now;
    }
}
