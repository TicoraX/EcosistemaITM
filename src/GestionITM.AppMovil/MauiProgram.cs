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
            // Emulador Android: 10.0.2.2 apunta al localhost del host. Dispositivo físico: IP LAN del PC.
            client.BaseAddress = new Uri("http://10.0.2.2:8080/");
        }).AddHttpMessageHandler<AuthHandler>();

        // Registrar Vistas y ViewModels (Inyección de Dependencias)
        builder.Services.AddTransient<LoginView>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<CursosView>();
        builder.Services.AddTransient<CursosViewModel>();
        builder.Services.AddTransient<ProfesoresView>();
        builder.Services.AddTransient<ProfesoresViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
