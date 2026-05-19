namespace GestionITM.AppMovil.Models;

public class CursoDto
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public int CuposDisponibles { get; set; }
}
