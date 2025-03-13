using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProyectoSano.Models;
using System.ComponentModel;

namespace ProyectoSano.Pages;

public partial class Login : ContentPage, INotifyPropertyChanged
{
    private readonly StructureService _structureService;
    private readonly string userFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "user.json");
    private readonly string apiUrl = "http://nekjoesg-001-site5.anytempurl.com/api";
    private HttpClient httpClient = new HttpClient();

    public Login()
    {
        InitializeComponent();
        _structureService = DependencyService.Get<StructureService>();
    }

    private void OnShowPasswordToggled(object sender, ToggledEventArgs e)
    {
        LoginPassword.IsPassword = !e.Value;
    }

    private async void LoginButtonClicked(object sender, EventArgs e)
    {
        try
        {
            HttpResponseMessage response = await httpClient.GetAsync($"{apiUrl}/User/login/{LoginEmail.Text}/{LoginPassword.Text}");

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();

                JObject jsonObject = JObject.Parse(jsonResponse);
                User user = jsonObject["user"].ToObject<User>();

                if (user != null)
                {
                    _structureService.User = user;
                    Serialize(user);

                    await Navigation.PushAsync(new Home2());
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo obtener al usuario", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "No se encontro al usuario", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error: {ex.Message}", "OK");
        }
    }

    private void Serialize(User user)
    {
        string json = JsonConvert.SerializeObject(user);
        File.WriteAllText(userFilePath, json);
    }
}