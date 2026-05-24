using System.Collections.ObjectModel;
using GestionITM.AppMovil.Models;

namespace GestionITM.AppMovil.ViewModels;

public class ApiExplorerViewModel
{
    public string Title { get; } = "Swagger móvil";

    public string Subtitle { get; } = "Una vista compacta para recordar qué endpoint usar en cada paso del flujo.";

    public string BaseUrlHint { get; } = "API local: http://10.0.2.2:5016/ | Docker: http://10.0.2.2:8080/";

    public ObservableCollection<ApiEndpointCard> Endpoints { get; } = new()
    {
        new ApiEndpointCard
        {
            Method = "POST",
            Path = "/api/Auth/login",
            Title = "Iniciar sesión",
            Description = "Devuelve el JWT y el estudianteId para guardar la sesión en SecureStorage.",
            AccessLabel = "Público",
            ExampleBody = "{\n  \"correo\": \"estudiante.demo@correo.itm.edu.co\",\n  \"password\": \"ItmDemo2026!\"\n}"
        },
        new ApiEndpointCard
        {
            Method = "POST",
            Path = "/api/Estudiante",
            Title = "Registrar estudiante",
            Description = "Crea un estudiante nuevo. El correo debe terminar en @correo.itm.edu.co.",
            AccessLabel = "Público",
            ExampleBody = "{\n  \"nombre\": \"Pablo\",\n  \"correo\": \"pablo@correo.itm.edu.co\",\n  \"password\": \"ItmDemo2026!\"\n}"
        },
        new ApiEndpointCard
        {
            Method = "GET",
            Path = "/api/Curso/paginado?pageNumber=1&pageSize=10",
            Title = "Listar cursos",
            Description = "Carga el catálogo paginado para el scroll infinito de la app.",
            AccessLabel = "Público"
        },
        new ApiEndpointCard
        {
            Method = "GET",
            Path = "/api/Profesor",
            Title = "Directorio de profesores",
            Description = "Devuelve la lista completa de profesores para la vista dedicada.",
            AccessLabel = "JWT"
        },
        new ApiEndpointCard
        {
            Method = "POST",
            Path = "/api/Matricula",
            Title = "Crear matrícula",
            Description = "Inscribe al estudiante autenticado en un curso disponible.",
            AccessLabel = "JWT",
            ExampleBody = "{\n  \"cursoId\": 4,\n  \"periodo\": \"2026-1\"\n}"
        },
        new ApiEndpointCard
        {
            Method = "GET",
            Path = "/api/Estudiante",
            Title = "Consultar estudiantes",
            Description = "Útil para probar el token y verificar el acceso protegido.",
            AccessLabel = "JWT"
        }
    };
}