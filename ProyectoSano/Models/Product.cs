namespace ProyectoSano.Models
{
    public class Product
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int Price { get; set; }
        public string Address { get; set; }
        public string Image { get; set; }
        public string[] Images { get; set; }
    }
}
