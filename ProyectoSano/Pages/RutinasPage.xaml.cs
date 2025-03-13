using ProyectoSano.Models;
using ProyectoSano.Service;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ProyectoSano.Pages;

public partial class RutinasPage : ContentPage, INotifyPropertyChanged
{
    private readonly StructureService _structureService;
    private readonly RutinaService _rutinaService;
    public ObservableCollection<Rutina> _ejerciciosList { get; set; }

    public ObservableCollection<Rutina> EjerciciosList
    {
        get => _ejerciciosList;
        set
        {
            _ejerciciosList = value;
            OnPropertyChanged(nameof(EjerciciosList));
        }
    }

    private ObservableCollection<Rutina> _ejerciciosFiltrado;
    public ObservableCollection<Rutina> EjerciciosFiltrado
    {
        get => _ejerciciosFiltrado;
        set
        {
            _ejerciciosFiltrado = value;
            OnPropertyChanged(nameof(EjerciciosFiltrado));
        }
    }

    public RutinasPage()
    {
        InitializeComponent();
        _structureService = DependencyService.Get<StructureService>();
        _rutinaService = new RutinaService();
        EjerciciosList = new ObservableCollection<Rutina>();
        _ = LoadRutinasAsync();

        BindingContext = this;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private async Task LoadRutinasAsync()
    {
        EjerciciosList = await _rutinaService.GetRutinaByUserIdAsync(_structureService.User.Id);
        FiltrarDia("Lunes");
    }

    private void FiltrarDia(string diaSeleccionado)
    {
        EjerciciosFiltrado = new ObservableCollection<Rutina>(_ejerciciosList.Where(r => r.Dia == diaSeleccionado).ToList());
    }

    private void OnDayTapped(object sender, EventArgs e)
    {
        lunesBorder.IsVisible = false;
        martesBorder.IsVisible = false;
        miercolesBorder.IsVisible = false;
        juevesBorder.IsVisible = false;
        viernesBorder.IsVisible = false;

        foreach (var child in days.Children)
        {
            if (child is Label label)
            {
                label.BackgroundColor = Colors.Transparent;
                label.TextColor = Color.FromHex("#ec268f");
            }
        }

        if (sender is Label selectedLabel)
        {
            switch (selectedLabel.Text)
            {
                case "Lunes":
                    lunesBorder.IsVisible = true;
                    break;
                case "Martes":
                    martesBorder.IsVisible = true;
                    break;
                case "Miercoles":
                    miercolesBorder.IsVisible = true;
                    break;
                case "Jueves":
                    juevesBorder.IsVisible = true;
                    break;
                case "Viernes":
                    viernesBorder.IsVisible = true;
                    break;
            }

            selectedLabel.BackgroundColor = Colors.White;
            selectedLabel.TextColor = Color.FromHex("#a8cf45");

            FiltrarDia(selectedLabel.Text);
        }
    }

    private async void redirectTapped(object sender, TappedEventArgs e)
    {
        var frame = (Frame)sender;
        var rutina = frame.BindingContext as Rutina;

        if (rutina != null)
        {
            await Navigation.PushAsync(new RutinaView(rutina));
        }
    }
}