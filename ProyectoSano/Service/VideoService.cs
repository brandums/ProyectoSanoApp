using ProyectoSano.Models;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace ProyectoSano.Service
{
    public class VideoService
    {
        private readonly HttpClient _httpClient;

        public VideoService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<ObservableCollection<Videos>> GetVideosAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<Videos>>($"http://nekjoesg-001-site5.anytempurl.com/api/Video");
            return response != null ? new ObservableCollection<Videos>(response) : new ObservableCollection<Videos>();
        }
    }
}
