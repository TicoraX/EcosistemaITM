using System.Collections.ObjectModel;
using System.Windows.Input;
using GestionITM.AppMovil.Models;
using GestionITM.AppMovil.Services;
using GestionITM.AppMovil.Views;

namespace GestionITM.AppMovil.ViewModels;

public class CursosViewModel : BindableObject
{
    private readonly ApiService _apiService;
    private readonly ApiExplorerView _apiExplorerView;
    private readonly ProfesoresView _profesoresView;
    private int _currentPage = 1;
    private bool _isLoading;

    public ObservableCollection<CursoDto> Cursos { get; set; } = new();
    
    public ICommand CargarMasCursosCommand { get; }
    public ICommand MatricularCommand { get; }
    public ICommand NavegarAProfesoresCommand { get; }
    public ICommand NavegarAApiExplorerCommand { get; }

    public CursosViewModel(ApiService apiService, ApiExplorerView apiExplorerView, ProfesoresView profesoresView)
    {
        _apiService = apiService;
        _apiExplorerView = apiExplorerView;
        _profesoresView = profesoresView;
        CargarMasCursosCommand = new Command(async () => await CargarCursosAsync());
        MatricularCommand = new Command<CursoDto>(async (c) => await MatricularseAsync(c));
        NavegarAProfesoresCommand = new Command(async () => await Application.Current!.MainPage!.Navigation.PushAsync(_profesoresView));
        NavegarAApiExplorerCommand = new Command(async () => await Application.Current!.MainPage!.Navigation.PushAsync(_apiExplorerView));
        
        // Carga Inicial
        _ = CargarCursosAsync();
    }

    private async Task CargarCursosAsync()
    {
        if (_isLoading) return;
        _isLoading = true;

        try
        {
            var result = await _apiService.GetCursosPaginadosAsync(_currentPage);
            
            // Si no hay más elementos, evitar seguir incrementando páginas
            if (result.Items == null || result.Items.Count == 0) return;

            foreach (var curso in result.Items)
            {
                Cursos.Add(curso);
            }
            _currentPage++;
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            _isLoading = false;
        }
    }

    private async Task MatricularseAsync(CursoDto curso)
    {
        if (curso == null) return;

        try
        {
            var estudianteId = await ApiService.GetEstudianteIdAsync();
            if (estudianteId <= 0)
            {
                await Application.Current!.MainPage!.DisplayAlert("Atención", "Debes iniciar sesión nuevamente.", "OK");
                return;
            }

            await _apiService.MatricularAsync(curso.Id);
            
            // Disminuir cupo visualmente para feedback inmediato
            if (curso.CuposDisponibles > 0)
            {
                curso.CuposDisponibles--;
                // Forzar actualización visual si fuera necesario (aquí es simplificado)
            }

            await Application.Current!.MainPage!.DisplayAlert("Matrícula Exitosa 🎉", $"Te has matriculado en el curso: {curso.Nombre}", "Excelente");
        }
        catch (MatriculaApiException ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("No fue posible matricular", ex.Message, "OK");
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
        }
    }
}
