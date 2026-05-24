using GestionITM.AppMovil.ViewModels;

namespace GestionITM.AppMovil.Views;

public partial class ApiExplorerView : ContentPage
{
    public ApiExplorerView(ApiExplorerViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}