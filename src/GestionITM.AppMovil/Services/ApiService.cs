using System.Net.Http.Json;
using GestionITM.AppMovil.Models;

namespace GestionITM.AppMovil.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> LoginAsync(string email, string password)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", new { email, password });
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
            if (result != null && !string.IsNullOrEmpty(result.Token))
            {
                // Guardar token en almacenamiento seguro nativo
                await SecureStorage.SetAsync("jwt_token", result.Token);
                return result.Token;
            }
        }
        throw new Exception("Correo electrónico o contraseña incorrectos.");
    }

    public async Task<PagedResult<CursoDto>> GetCursosPaginadosAsync(int page)
    {
        var response = await _httpClient.GetAsync($"api/curso/paginado?pageNumber={page}&pageSize=10");
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<PagedResult<CursoDto>>();
            return result ?? new PagedResult<CursoDto>();
        }
        throw new Exception("No se pudo cargar el catálogo de cursos.");
    }

    public async Task MatricularAsync(int estudianteId, int cursoId)
    {
        var response = await _httpClient.PostAsJsonAsync("api/matricula", new 
        { 
            EstudianteId = estudianteId, 
            CursoId = cursoId, 
            Periodo = "2026-1" 
        });

        if (!response.IsSuccessStatusCode)
        {
            try
            {
                // Leer el error devuelto por la regla de negocio del backend
                var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                throw new Exception(error?.Message ?? "No fue posible realizar la matrícula.");
            }
            catch (JsonException)
            {
                throw new Exception("Error inesperado en el servidor al matricularse.");
            }
        }
    }
}

public class TokenResponse 
{ 
    public string Token { get; set; } = string.Empty; 
}

public class ErrorResponse 
{ 
    public string Message { get; set; } = string.Empty; 
}
