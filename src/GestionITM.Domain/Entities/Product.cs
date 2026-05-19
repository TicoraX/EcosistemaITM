using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GestionITM.Domain.Entities
{
    public class Product
    {
        // EF Core reconocerá "Id" como clave primaria
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Descripcion { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        // Le indicamos explícitamente a SQL Server cuántos dígitos totales y decimales usar.
        [System.ComponentModel.DataAnnotations.Schema.Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    }
}
