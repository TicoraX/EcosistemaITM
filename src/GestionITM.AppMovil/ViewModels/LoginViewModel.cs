using System.Windows.Input;
using GestionITM.AppMovil.Services;
using GestionITM.AppMovil.Views;

namespace GestionITM.AppMovil.ViewModels;

public class LoginViewModel : BindableObject
{
    private readonly ApiService _apiService;
    private readonly CursosView _cursosView;
    private string _email = string.Empty;
    private string _password = string.Empty;
    private bool _isBusy;

    public string Email
    {
        get => _email;
        set { _email = value; OnPropertyChanged(); }
    }

    public string Password
    {
        get => _password;
        set { _password = value; OnPropertyChanged(); }
    }

    public bool IsBusy
    {
        get => _isBusy;
        set { _isBusy = value; OnPropertyChanged(); }
    }

    public ICommand LoginCommand { get; }

    public LoginViewModel(ApiService apiService, CursosView cursosView)
    {
        _apiService = apiService;
        _cursosView = cursosView;
        LoginCommand = new Command(async () => await IniciarSesionAsync(), () => !IsBusy);
    }

    private async Task IniciarSesionAsync()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            await Application.Current!.MainPage!.DisplayAlert("Atención", "Por favor ingresa tu correo y contraseña.", "Entendido");
            return;
        }

        IsBusy = true;
        ((Command)LoginCommand).ChangeCanExecute();

        try
        {
            await _apiService.LoginAsync(Email, Password);
            
            // Navegar a la pantalla del catálogo de cursos de forma exitosa
            Application.Current!.MainPage = new NavigationPage(_cursosView);
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error de Acceso ❌", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
            ((Command)LoginCommand).ChangeCanExecute();
        }
    }
}
