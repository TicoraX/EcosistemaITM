namespace GestionITM.AppMovil;

public partial class App : Application
{
    public App(Views.LoginView loginView)
    {
        InitializeComponent();

        // Inicializar la aplicación móvil en la vista de Login
        MainPage = new NavigationPage(loginView);
    }
}
