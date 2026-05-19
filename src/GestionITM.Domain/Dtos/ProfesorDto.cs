namespace GestionITM.Domain.Dtos
{
    // DTO de lectura: no exponemos FechaContratacion hacia el cliente
    public class ProfesorDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Especialidad { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
