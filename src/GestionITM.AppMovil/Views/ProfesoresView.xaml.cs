using GestionITM.AppMovil.ViewModels;

namespace GestionITM.AppMovil.Views;

/// <summary>
/// Código subyacente (code-behind) de ProfesoresView. 
/// Actúa como la Marioneta del show de títeres, sin lógica de negocio, 
/// inyectando su cerebro (ViewModel) mediante Inyección de Dependencias.
/// </summary>
public partial class ProfesoresView : ContentPage
{
    public ProfesoresView(ProfesoresViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
