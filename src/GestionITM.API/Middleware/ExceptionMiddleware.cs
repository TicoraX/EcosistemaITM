using GestionITM.Domain.Exceptions;
using GestionITM.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Net;
using System.Text.Json;

namespace GestionITM.API.Middleware
{
    // Nota pedagógica:
    // Este middleware vive en la capa API porque:
    // - Trabaja directamente con HttpContext, RequestDelegate y el pipeline HTTP de ASP.NET Core.
    // - Forma parte de la capa de presentación: se encarga de cómo respondemos al cliente (status codes, JSON de error).
    // La capa Infrastructure se enfoca en acceso a datos (DbContext, repositorios) y no debería depender de ASP.NET Core.
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next; // El siguiente paso en la tubería
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Intentar seguir el flujo normal
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                if (ex is NoAvailableSeatsException or CursoNotFoundException)
                    Log.Warning(ex, "Regla de negocio: {Message}", ex.Message);
                else
                    Log.Error(ex, "Error no controlado: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            // ANTES EN CLASE: siempre devolvíamos 500 para cualquier excepción
            //   context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //   var response = new ErrorResponse
            //   {
            //       StatusCode = context.Response.StatusCode,
            //       Message = "Ocurrió un error interno en el servidor del ITM.",
            //       Details = _env.IsDevelopment() ? ex.StackTrace?.ToString() : null
            //   };
            // RETO: ahora usamos un switch para personalizar el StatusCode según el tipo de excepción

            // Determinar el código de estado según el tipo de excepción
            var statusCode = ex switch
            {
                NoAvailableSeatsException => (int)HttpStatusCode.BadRequest,
                CursoNotFoundException => (int)HttpStatusCode.BadRequest,
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                ArgumentException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError
            };
            context.Response.StatusCode = statusCode;

            var response = new ErrorResponse
            {
                StatusCode = statusCode,
                Message = ex switch
                {
                    NoAvailableSeatsException noSeats => noSeats.Message,
                    CursoNotFoundException curso => curso.Message,
                    KeyNotFoundException => "El recurso solicitado no fue encontrado en el sistema del ITM.",
                    ArgumentException arg => arg.Message,
                    _ => statusCode == (int)HttpStatusCode.InternalServerError
                        ? "Ocurrió un error interno en el servidor del ITM."
                        : "La petición enviada no es válida. Verifique los datos."
                },
                // Si estamos en desarrollo, mostramos el error real. En producción, no.
                Details = _env.IsDevelopment() ? ex.StackTrace?.ToString() : null
            };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
    }
}