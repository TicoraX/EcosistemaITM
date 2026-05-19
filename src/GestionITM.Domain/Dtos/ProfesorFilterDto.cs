using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionITM.Domain.Dtos
{
    public class ProfesorFilterDto
    {
        public int Pagina { get; set; } = 1; // Página por defecto
        public int RegistrosPorPagina { get; set; } = 10; // Registros por página por defecto
        public string? BusquedaNombre { get; set; } // Filtro opcional por nombre
        public string? Especialidad { get; set; } // Filtro opcional por especialidad
    }
}
