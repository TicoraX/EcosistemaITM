using System.Net.Http.Json;
using System.Text.Json;
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
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", new { Email = email, Password = password });
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
            if (result != null && !string.IsNullOrEmpty(result.Token))
            {
                // Guardar token en almacenamiento seguro nativo
                await SecureStorage.SetAsync("auth_token", result.Token);
                if (result.EstudianteId > 0)
                    await SecureStorage.SetAsync("estudiante_id", result.EstudianteId.ToString());
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

    public async Task MatricularAsync(int cursoId)
    {
        var response = await _httpClient.PostAsJsonAsync("api/matricula", new 
        { 
            CursoId = cursoId, 
            Periodo = "2026-1" 
        });

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await TryReadErrorMessageAsync(response);
            throw new MatriculaApiException(errorMessage);
        }
    }

    public static async Task<int> GetEstudianteIdAsync()
    {
        var id = await SecureStorage.GetAsync("estudiante_id");
        return int.TryParse(id, out var parsed) ? parsed : 0;
    }

    private static async Task<string> TryReadErrorMessageAsync(HttpResponseMessage response)
    {
        try
        {
            var body = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(body))
                return "No fue posible realizar la matrícula.";

            var error = JsonSerializer.Deserialize<ErrorResponse>(body,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (!string.IsNullOrWhiteSpace(error?.Message))
                return error.Message;

            using var doc = JsonDocument.Parse(body);
            if (doc.RootElement.TryGetProperty("message", out var msg))
                return msg.GetString() ?? "No fue posible realizar la matrícula.";
        }
        catch
        {
            // ignored
        }

        return "No fue posible realizar la matrícula.";
    }

    public async Task<List<ProfesorModel>> GetProfesoresAsync()
    {
        var response = await _httpClient.GetAsync("api/profesor");
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<List<ProfesorModel>>();
            return result ?? new List<ProfesorModel>();
        }
        throw new Exception("No se pudo obtener el directorio de profesores.");
    }
}

public class TokenResponse
{
    public string Token { get; set; } = string.Empty;
    public int EstudianteId { get; set; }
}

public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;
}

public class MatriculaApiException : Exception
{
    public MatriculaApiException(string message) : base(message) { }
}
