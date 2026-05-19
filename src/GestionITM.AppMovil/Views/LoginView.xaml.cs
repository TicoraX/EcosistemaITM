using GestionITM.AppMovil.ViewModels;

namespace GestionITM.AppMovil.Views;

public partial class LoginView : ContentPage
{
    public LoginView(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
