namespace GestionITM.Domain.Dtos
{
    // DTO de creación: los datos que el cliente envía para registrar un profesor
    public class ProfesorCreateDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Especialidad { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
