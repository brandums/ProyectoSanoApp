using ProyectoSano.Models;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace ProyectoSano.Service
{
    public class ProductoService
    {
        private readonly HttpClient _httpClient;

        public ProductoService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<ObservableCollection<Productos>> GetProductosByCategoriaAsync(string categoria)
        {
            var response = await _httpClient.GetFromJsonAsync<List<Productos>>($"http://nekjoesg-001-site5.anytempurl.com/api/Producto/Categoria/{categoria}");
            return response != null ? new ObservableCollection<Productos>(response) : new ObservableCollection<Productos>();
        }
    }
}
