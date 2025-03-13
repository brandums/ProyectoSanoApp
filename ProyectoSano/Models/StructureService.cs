using Newtonsoft.Json;

namespace ProyectoSano.Models
{
    public class StructureService
    {
        public User User { get; set; }
        private readonly HttpClient _httpClient;
        private readonly string apiUrl = "http://nekjoesg-001-site5.anytempurl.com/api";
        private readonly string UserFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "user.json");

        public StructureService()
        {
            _httpClient = new HttpClient();

            LoadLocalUserData();
        }

        private void LoadLocalUserData()
        {
            if (File.Exists(UserFilePath))
            {
                string userJson = File.ReadAllText(UserFilePath);
                User = JsonConvert.DeserializeObject<User>(userJson);
            }
        }

        public async Task<bool> CheckAndUpdateData()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                await UpdateUserData();

                return true;
            }

            return false;
        }

        public async Task UpdateUserData()
        {
            if (User == null) return;

            try
            {
                var response = await _httpClient.GetAsync($"{apiUrl}/User/{User.Id}");
                if (response.IsSuccessStatusCode)
                {
                    string updatedUserJson = await response.Content.ReadAsStringAsync();
                    File.WriteAllText(UserFilePath, updatedUserJson);
                    User = JsonConvert.DeserializeObject<User>(updatedUserJson);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar el usuario: {ex.Message}");
            }
        }
    }
}
