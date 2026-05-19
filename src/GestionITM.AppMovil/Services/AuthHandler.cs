using System.Net.Http.Headers;

namespace GestionITM.AppMovil.Services;

public class AuthHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Recuperar el token JWT de SecureStorage de forma segura y multiplataforma
        var token = await SecureStorage.GetAsync("jwt_token");
        
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
