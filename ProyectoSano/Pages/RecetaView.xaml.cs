using ProyectoSano.Models;

namespace ProyectoSano.Pages;

public partial class RecetaView : ContentPage
{
    public RecetaView(Receta receta)
    {
        InitializeComponent();
        BindingContext = receta;
    }
}