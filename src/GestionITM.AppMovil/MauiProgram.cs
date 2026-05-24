using GestionITM.AppMovil.Services;
using GestionITM.AppMovil.ViewModels;
using GestionITM.AppMovil.Views;
using Microsoft.Extensions.Logging;

namespace GestionITM.AppMovil;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Registrar el Interceptor HTTP
        builder.Services.AddTransient<AuthHandler>();

        // Registrar el ApiService configurado con el AuthHandler para inyectar JWT automáticamente
        builder.Services.AddHttpClient<ApiService>(client =>
        {
            // Emulador Android: 10.0.2.2 = localhost del PC.
            // dotnet run (API local) → puerto 5016 | Docker → puerto 8080 | Dispositivo físico → IP LAN del PC.
            client.BaseAddress = new Uri(GetApiBaseUrl());
        }).AddHttpMessageHandler<AuthHandler>();

        // Registrar Vistas y ViewModels (Inyección de Dependencias)
        builder.Services.AddTransient<LoginView>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<ApiExplorerView>();
        builder.Services.AddTransient<ApiExplorerViewModel>();
        builder.Services.AddTransient<CursosView>();
        builder.Services.AddTransient<CursosViewModel>();
        builder.Services.AddTransient<ProfesoresView>();
        builder.Services.AddTransient<ProfesoresViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    private static string GetApiBaseUrl()
    {
        // Cambie el puerto según cómo ejecute la API: 5016 (dotnet run) o 8080 (docker compose).
        const string apiPort = "5016";
        return $"http://10.0.2.2:{apiPort}/";
    }
}
