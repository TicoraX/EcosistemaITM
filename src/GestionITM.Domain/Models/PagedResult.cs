using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionITM.Domain.Models
{
    // Un contenedor generico para cualquier lista paginada. T es el tipo de dato que se pagina.
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new (); // La lista de elementos que se mostrarán en la página actual
        public int PaginaActual { get; set; } // El número de la página actual
        public int TotalPaginas { get; set; } // El número total de páginas disponibles
        public int TotalRegistros { get; set; } // El número total de registros en la base de datos (sin paginar)
        public int RegistrosPorPagina { get; set; } // El número de registros que se muestran por página
    }
}
