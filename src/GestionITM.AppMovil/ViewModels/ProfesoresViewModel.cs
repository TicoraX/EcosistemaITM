using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionITM.AppMovil.Models;
using GestionITM.AppMovil.Services;

namespace GestionITM.AppMovil.ViewModels;

/// <summary>
/// Cerebro de la pantalla de profesores (Titiritero), siguiendo el patrón MVVM
/// y utilizando CommunityToolkit.Mvvm con generadores de código.
/// </summary>
public partial class ProfesoresViewModel : ObservableObject
{
    private readonly ApiService _apiService;

    // Regla de Oro 3: Variables privadas en minúscula con [ObservableProperty] 
    // para que el compilador autogenere la propiedad pública reactiva.
    [ObservableProperty]
    private string tituloPantalla = "Directorio de Profesores ITM";

    [ObservableProperty]
    private bool estaCargando;

    // Regla de Oro 2: Usar una colección observable para notificar automáticamente a la vista
    public ObservableCollection<ProfesorModel> ListaProfesores { get; set; } = new();

    public ProfesoresViewModel(ApiService apiService)
    {
        _apiService = apiService;
        
        // Carga inicial al inicializar
        _ = CargarProfesoresAsync();
    }

    // Regla de Oro 4: Los botones/acciones llaman comandos. 
    // [RelayCommand] genera CargarProfesoresCommand de manera automática.
    [RelayCommand]
    private async Task CargarProfesoresAsync()
    {
        if (EstaCargando) return;
        EstaCargando = true;

        try
        {
            ListaProfesores.Clear();
            var profesores = await _apiService.GetProfesoresAsync();
            foreach (var prof in profesores)
            {
                ListaProfesores.Add(prof);
            }
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            EstaCargando = false;
        }
    }
}
