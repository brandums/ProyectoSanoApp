namespace ProyectoSano.Models
{
    public class Receta
    {
        public string Nombre { get; set; }
        public string Subtitulo { get; set; }
        public string[] Ingredientes { get; set; }
        public string[] Preparacion { get; set; }
        public string Categoria { get; set; }
        public string Imagen { get; set; }
    }
}

