using MarkAddIn;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms.Design;
using System.Windows.Navigation;


//Mark.Client espacio de nombres: clase de donde se van a referenciar los servicios de la aplicación
namespace Mark.Client
{
    public partial class Bootstrapper  
        
        //correción: es necesario que la clase sea parcial para que se puedan referenciar los distintos servicios con los que se cuenta           
    {
        public static IServiceProvider ConfigureServices()      
        {
            var servicios = new ServiceCollection();         

            // Servicios principales
            servicios.AddSingleton<AuthenticationManager>();
            servicios.AddSingleton<TokenManager>();
            servicios.AddSingleton<TokenHander>();


            servicios.AddSingleton<MarkRibbon>();
            servicios.AddSingleton<MainWindow>();

         
            return servicios.BuildServiceProvider();
        }
    }
}

