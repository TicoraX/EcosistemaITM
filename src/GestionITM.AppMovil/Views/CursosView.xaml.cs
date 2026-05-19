using GestionITM.AppMovil.ViewModels;

namespace GestionITM.AppMovil.Views;

public partial class CursosView : ContentPage
{
    public CursosView(CursosViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
