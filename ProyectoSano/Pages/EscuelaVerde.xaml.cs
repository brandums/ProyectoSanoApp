using ProyectoSano.Models;
using ProyectoSano.Service;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ProyectoSano.Pages;

public partial class EscuelaVerde : ContentPage, INotifyPropertyChanged
{
    private readonly VideoService _videoService;
    public ObservableCollection<Videos> _videosList { get; set; }

    public ObservableCollection<Videos> VideosList
    {
        get => _videosList;
        set
        {
            _videosList = value;
            OnPropertyChanged(nameof(VideosList));
        }
    }

    public EscuelaVerde()
    {
        InitializeComponent();
        _videoService = new VideoService();

        VideosList = new ObservableCollection<Videos>();
        BindingContext = this;

        _ = LoadVideosAsync();
    }

    private async Task LoadVideosAsync()
    {
        VideosList = await _videoService.GetVideosAsync();
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private async void redirectTapped(object sender, TappedEventArgs e)
    {
        var link = e.Parameter as string;

        await Browser.OpenAsync(link, BrowserLaunchMode.SystemPreferred);
    }
}