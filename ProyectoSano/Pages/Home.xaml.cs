using ProyectoSano.Models;
using System.Collections.ObjectModel;

namespace ProyectoSano.Pages;

public partial class Home : ContentPage
{
    public List<Category> Categories { get; set; }
    public List<Product> Products { get; set; }

    public ObservableCollection<Product> FilteredProducts { get; set; }

    public Home()
    {
        InitializeComponent();
        LoadCategories();
        LoadProducts();

        FilteredProducts = new ObservableCollection<Product>(Products);

        BindingContext = this;
    }

    private void LoadCategories()
    {
        Categories = new List<Category>
        {
            new Category { Name = "Cocina", Title = "Cocina sana" },
            new Category { Name = "Escuela", Title = "Escuela verde" },
            new Category { Name = "Dieta", Title = "Dieta saludable" },
            new Category { Name = "Placeres", Title = "Placeres de la vida" },
            new Category { Name = "Problemas", Title = "Problemas de la vida" },
            new Category { Name = "Nutricionista", Title = "Recomendaciónes del nutricionista" },
            new Category { Name = "Gym", Title = "Gym en casa" }
        };
    }

    private void LoadProducts()
    {
        Products = new List<Product>
        {
            new Product { Title = "Producto 1", Subtitle = "Sub 1", Description = "Descripción del producto 1", Category = "Cocina", Price = 100, Address = "Dirección 1", Image = "https://imagetesteo1.blob.core.windows.net/product1/89c7c70a-f5a4-4014-a6df-e27da6e71def_hamburguesa.jpeg", Images = new string[] {} },
            new Product { Title = "Producto 2", Subtitle = "Sub 2", Description = "Descripción del producto 2", Category = "Escuela", Price = 200, Address = "Dirección 2", Image = "https://imagetesteo1.blob.core.windows.net/product1/ab647256-62d7-4bd2-b50d-d04fe7565c46_pollo.jpg", Images = new string[] {} },
            new Product { Title = "Producto 3", Subtitle = "Sub 3", Description = "Descripción del producto 3", Category = "Dieta", Price = 150, Address = "Dirección 3", Image = "https://imagetesteo1.blob.core.windows.net/product1/3d5256e9-c6c6-46e3-9fcc-bc536c3446d6_pizza.jpg", Images = new string[] {} },
            new Product { Title = "Producto 4", Subtitle = "Sub 4", Description = "Descripción del producto 4", Category = "Placeres", Price = 250, Address = "Dirección 4", Image = "https://imagetesteo1.blob.core.windows.net/product1/5a8a9948-eccb-49e9-84f9-0a67e6c9b988_carnes.jpeg", Images = new string[] {} },
            new Product { Title = "Producto 5", Subtitle = "Sub 5", Description = "Descripción del producto 5", Category = "Problemas", Price = 300, Address = "Dirección 5", Image = "https://imagetesteo1.blob.core.windows.net/product1/9f34b750-4d05-474c-b25f-1a6638158edc_pescado.jpeg", Images = new string[] {} },
            new Product { Title = "Producto 6", Subtitle = "Sub 6", Description = "Descripción del producto 6", Category = "Nutricionista", Price = 120, Address = "Dirección 6", Image = "https://imagetesteo1.blob.core.windows.net/product1/edc4a538-9a37-47b8-904f-ef79bbd03668_lasa\u00F1a.jpeg", Images = new string[] {} },
            new Product { Title = "Producto 7", Subtitle = "Sub 7", Description = "Descripción del producto 7", Category = "Cocina", Price = 180, Address = "Dirección 7", Image = "https://imagetesteo1.blob.core.windows.net/product1/6e7e749e-049c-47a7-b7b5-af9387286d0c_pique.jpg", Images = new string[] {} },
            new Product { Title = "Producto 8", Subtitle = "Sub 8", Description = "Descripción del producto 8", Category = "Cocina", Price = 90, Address = "Dirección 8", Image = "https://imagetesteo1.blob.core.windows.net/product1/89c7c70a-f5a4-4014-a6df-e27da6e71def_hamburguesa.jpeg", Images = new string[] {} }
        };
    }

    private void OnCategoryTapped(object sender, EventArgs e)
    {
        var frame = sender as Frame;

        var selectedCategory = frame.BindingContext as Category;

        if (selectedCategory != null)
        {
            var filtered = Products.Where(p => p.Category == selectedCategory.Name).ToList();
            FilteredProducts.Clear();
            foreach (var product in filtered)
            {
                FilteredProducts.Add(product);
            }

            ProductList.ItemsSource = FilteredProducts;
        }
    }

    private async void OnProductTapped(object sender, EventArgs e)
    {
        var frame = sender as Frame;
        var selectedProduct = frame.BindingContext as Product;

        if (selectedProduct != null)
        {
            // Navega a la página ProductView y pasa el producto seleccionado
            await Navigation.PushAsync(new ProductView(selectedProduct));
        }
    }

}