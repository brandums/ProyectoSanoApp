using ProyectoSano.Models;
using ProyectoSano.Pages;

namespace ProyectoSano
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var structureService = new StructureService();
            DependencyService.RegisterSingleton<StructureService>(structureService);

            MainPage = new NavigationPage(new Home2());
        }
    }
}
