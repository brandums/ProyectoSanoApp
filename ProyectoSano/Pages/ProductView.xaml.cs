using ProyectoSano.Models;

namespace ProyectoSano.Pages;

public partial class ProductView : ContentPage
{
    public ProductView(Product product)
    {
        InitializeComponent();
        BindingContext = product;
    }

    private async void OnBackButtonTapped(object sender, EventArgs e)
    {
        if (Navigation.NavigationStack.Count > 1)
        {
            await Navigation.PopAsync();
        }
    }
}