using ProyectoSano.Models;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace ProyectoSano.Service
{
    public class RutinaService
    {
        private readonly HttpClient _httpClient;

        public RutinaService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<ObservableCollection<Rutina>> GetRutinaByUserIdAsync(int userId)
        {
            var response = await _httpClient.GetFromJsonAsync<List<Rutina>>($"http://nekjoesg-001-site5.anytempurl.com/api/Rutina/userId/{userId}");
            return response != null ? new ObservableCollection<Rutina>(response) : new ObservableCollection<Rutina>();
        }
    }
}
