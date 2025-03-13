using ProyectoSano.Models;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace ProyectoSano.Service
{
    public class AlimentacionService
    {
        private readonly HttpClient _httpClient;

        public AlimentacionService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<ObservableCollection<Alimentacion>> GetAlimentacionByUserIdAsync(int userId)
        {
            var response = await _httpClient.GetFromJsonAsync<List<Alimentacion>>($"http://nekjoesg-001-site5.anytempurl.com/api/Alimentacion/userId/{userId}");
            return response != null ? new ObservableCollection<Alimentacion>(response) : new ObservableCollection<Alimentacion>();
        }
    }
}
