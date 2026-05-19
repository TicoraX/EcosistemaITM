using System.Collections.ObjectModel;
using System.Windows.Input;
using GestionITM.AppMovil.Models;
using GestionITM.AppMovil.Services;

namespace GestionITM.AppMovil.ViewModels;

public class CursosViewModel : BindableObject
{
    private readonly ApiService _apiService;
    private int _currentPage = 1;
    private bool _isLoading;

    public ObservableCollection<CursoDto> Cursos { get; set; } = new();
    
    public ICommand CargarMasCursosCommand { get; }
    public ICommand MatricularCommand { get; }

    public CursosViewModel(ApiService apiService)
    {
        _apiService = apiService;
        CargarMasCursosCommand = new Command(async () => await CargarCursosAsync());
        MatricularCommand = new Command<CursoDto>(async (c) => await MatricularseAsync(c));
        
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
            // Para la demostración se simula que el estudiante matriculado es el ID 1
            await _apiService.MatricularAsync(1, curso.Id);
            
            // Disminuir cupo visualmente para feedback inmediato
            if (curso.CuposDisponibles > 0)
            {
                curso.CuposDisponibles--;
                // Forzar actualización visual si fuera necesario (aquí es simplificado)
            }

            await Application.Current!.MainPage!.DisplayAlert("Matrícula Exitosa 🎉", $"Te has matriculado en el curso: {curso.Nombre}", "Excelente");
        }
        catch (Exception ex)
        {
            // FASE B: UX Resiliente. En caso de error 400 (ej: Falta de cupo), mostramos DisplayAlert sin cerrar la App.
            await Application.Current!.MainPage!.DisplayAlert("Fallo en la Matrícula ❌", ex.Message, "Entendido");
        }
    }
}
