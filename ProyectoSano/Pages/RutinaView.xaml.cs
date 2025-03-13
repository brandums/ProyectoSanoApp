using ProyectoSano.Models;

namespace ProyectoSano.Pages;

public partial class RutinaView : ContentPage
{
    public RutinaView(Rutina rutina)
    {
        InitializeComponent();
        BindingContext = rutina;
    }
}