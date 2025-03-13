using ProyectoSano.Models;

namespace ProyectoSano.Pages;

public partial class Home2 : ContentPage
{
    private readonly StructureService _structureService;
    private User user;
    private readonly string UserFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "user.json");

    public Home2()
    {
        InitializeComponent();
        _structureService = DependencyService.Get<StructureService>();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        CheckConnectivity();
    }

    private void changeUserView()
    {
        if (user != null)
        {
            Logout.IsVisible = true;
            Login.IsVisible = false;
            userName.Text = user.Name;

            if (user.CI == "1")
            {
                Admin.IsVisible = true;
            }
        }
    }

    private async void CheckConnectivity()
    {
        var isConnected = await _structureService.CheckAndUpdateData();
        if (!isConnected)
        {
            await DisplayAlert("Sin conexión a Internet", "No hay conexión a Internet. Algunos datos pueden no estar actualizados.", "OK");
        }

        user = _structureService.User;

        changeUserView();
    }

    private async void redirectTapped(object sender, TappedEventArgs e)
    {
        var targetPage = e.Parameter as string;

        if (targetPage == "RecetasPage")
        {
            await Navigation.PushAsync(new RecetasPage());
        }
        else if (targetPage == "Retos")
        {
            await Navigation.PushAsync(new Retos());
        }
        else if (targetPage == "Rutinas")
        {
            await Navigation.PushAsync(new RutinasPage());
        }
        else if (targetPage == "Escuela")
        {
            await Navigation.PushAsync(new EscuelaVerde());
        }
        else if (targetPage == "Talleres")
        {
            await Navigation.PushAsync(new Talleres());
        }
        else if (targetPage == "Alimentacion")
        {
            await Navigation.PushAsync(new PlanAlimentacion());
        }
        else if (targetPage == "Placeres")
        {
            await Navigation.PushAsync(new Placeres());
        }
        else if (targetPage == "Problemas")
        {
            await Navigation.PushAsync(new Problemas());
        }
    }

    private async void redirectWhatsappTapped(object sender, TappedEventArgs e)
    {
        string phoneNumber = "+59175926215";
        string whatsappUrl = $"https://wa.me/{phoneNumber}";
        await Launcher.OpenAsync(whatsappUrl);
    }

    private async void OnMenuIconTapped(object sender, EventArgs e)
    {
        Menu.IsVisible = true;
        await Menu.TranslateTo(0, 0, 250, Easing.SinIn);
    }

    private async void OnMenuBackgroundTapped(object sender, EventArgs e)
    {
        await Menu.TranslateTo(Menu.Width, 0, 250, Easing.SinIn);
        Menu.IsVisible = false;
    }

    private async void OnMenuItemTapped(object sender, EventArgs e)
    {
        var label = sender as Label;
        string route = string.Empty;

        if (label.Text == "Administracion")
        {
            await Navigation.PushAsync(new AdminPage());
        }
        else if (label.Text == "Iniciar Sesion")
        {
            await Navigation.PushAsync(new Login());
        }

        if (label.Text == "Logout")
        {
            deleteUserSerialize(route);
        }

        OnMenuBackgroundTapped(sender, e);
    }

    private async void deleteUserSerialize(string route)
    {
        if (File.Exists(UserFilePath))
        {
            try
            {
                File.Delete(UserFilePath);
                _structureService.User = null;
                user = null;

                Admin.IsVisible = false;
                Logout.IsVisible = false;
                Login.IsVisible = true;
                userName.Text = string.Empty;
            }
            catch (Exception ex)
            {

            }
        }
    }
}