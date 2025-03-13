using ProyectoSano.Models;
using ProyectoSano.Service;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ProyectoSano.Pages;

public partial class PlanAlimentacion : ContentPage, INotifyPropertyChanged
{
    private readonly StructureService _structureService;
    public ObservableCollection<Alimentacion> _alimentacionDias { get; set; }
    private AlimentacionService _alimentacionService;

    public ObservableCollection<Alimentacion> AlimentacionDias
    {
        get => _alimentacionDias;
        set
        {
            _alimentacionDias = value;
            OnPropertyChanged(nameof(AlimentacionDias));
        }
    }

    public PlanAlimentacion()
    {
        InitializeComponent();

        _alimentacionService = new AlimentacionService();
        _structureService = DependencyService.Get<StructureService>();
        AlimentacionDias = new ObservableCollection<Alimentacion>();
        BindingContext = this;

        _ = CargarAlimentacionAsync();
    }

    private async Task CargarAlimentacionAsync()
    {
        AlimentacionDias = await _alimentacionService.GetAlimentacionByUserIdAsync(_structureService.User.Id);
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}