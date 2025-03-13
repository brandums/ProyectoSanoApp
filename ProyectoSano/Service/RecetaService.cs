using ProyectoSano.Models;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace ProyectoSano.Service
{
    public class RecetaService
    {
        private readonly HttpClient _httpClient;

        public RecetaService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<ObservableCollection<Receta>> GetRecetasByCategoriaAsync(string categoria)
        {
            var response = await _httpClient.GetFromJsonAsync<List<Receta>>($"http://nekjoesg-001-site5.anytempurl.com/api/Receta/Categoria/{categoria}");
            return response != null ? new ObservableCollection<Receta>(response) : new ObservableCollection<Receta>();
        }
    }
}
