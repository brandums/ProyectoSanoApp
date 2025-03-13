using ProyectoSano.Models;
using ProyectoSano.Service;
using System.Collections.ObjectModel;

namespace ProyectoSano.Pages;

public partial class RecetasPage : ContentPage
{
    private readonly RecetaService _recetaService;
    public ObservableCollection<Receta> RecetasFiltradas { get; set; } = new ObservableCollection<Receta>();

    public RecetasPage()
    {
        InitializeComponent();
        _recetaService = new RecetaService();

        _ = FiltrarRecetas("Desayunos");

        BindingContext = this;
    }

    private async Task FiltrarRecetas(string categoriaSeleccionada)
    {
        RecetasFiltradas.Clear();

        var recetas = await _recetaService.GetRecetasByCategoriaAsync(categoriaSeleccionada);
        foreach (var receta in recetas)
        {
            RecetasFiltradas.Add(receta);
        }
    }

    private void OnCategoryTapped(object sender, EventArgs e)
    {
        desayunosBorder.IsVisible = false;
        almuerzosBorder.IsVisible = false;
        ensaladasBorder.IsVisible = false;
        postresBorder.IsVisible = false;
        cenasBorder.IsVisible = false;
        otrosBorder.IsVisible = false;

        foreach (var child in categories.Children)
        {
            if (child is Label label)
            {
                label.BackgroundColor = Colors.Transparent; // Fondo transparente
                label.TextColor = Color.FromHex("#ec268f"); // Color original de texto
            }
        }

        if (sender is Label selectedLabel)
        {
            switch (selectedLabel.Text)
            {
                case "Desayunos":
                    desayunosBorder.IsVisible = true;
                    break;
                case "Almuerzos":
                    almuerzosBorder.IsVisible = true;
                    break;
                case "Ensaladas":
                    ensaladasBorder.IsVisible = true;
                    break;
                case "Postres":
                    postresBorder.IsVisible = true;
                    break;
                case "Cenas":
                    cenasBorder.IsVisible = true;
                    break;
                case "Otros":
                    otrosBorder.IsVisible = true;
                    break;
            }

            selectedLabel.BackgroundColor = Colors.White;
            selectedLabel.TextColor = Color.FromHex("#a8cf45");

            FiltrarRecetas(selectedLabel.Text);
        }
    }

    private async void redirectTapped(object sender, TappedEventArgs e)
    {
        var frame = (Frame)sender;
        var receta = frame.BindingContext as Receta;

        if (receta != null)
        {
            await Navigation.PushAsync(new RecetaView(receta));
        }
    }
}