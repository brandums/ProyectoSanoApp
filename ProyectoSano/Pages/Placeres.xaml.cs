using ProyectoSano.Models;
using ProyectoSano.Service;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ProyectoSano.Pages;

public partial class Placeres : ContentPage, INotifyPropertyChanged
{
    private readonly ProductoService _productoService;
    public ObservableCollection<Productos> _productosList { get; set; }

    public ObservableCollection<Productos> ProductosList
    {
        get => _productosList;
        set
        {
            _productosList = value;
            OnPropertyChanged(nameof(ProductosList));
        }
    }

    public Placeres()
    {
        InitializeComponent();
        _productoService = new ProductoService();

        ProductosList = new ObservableCollection<Productos>();
        BindingContext = this;

        _ = LoadProductosAsync();
    }

    private async Task LoadProductosAsync()
    {
        ProductosList = await _productoService.GetProductosByCategoriaAsync("Gustos");
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}