namespace GestionITM.AppMovil.Models;

public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int PaginaActual { get; set; }
    public int TotalPaginas { get; set; }
}
