using MarkAddIn;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Office.Interop.Excel;
using System;
using System.Windows.Forms.Design;
using System.Windows.Navigation;


//Mark.Client espacio de nombres: clase de donde se van a referenciar los servicios de la aplicación
namespace Mark.Client
{
    public partial class Bootstrapper  
        
        //correción: es necesario que la clase sea parcial para que se puedan referenciar los distintos servicios con los que se cuenta           
    {
        public static IServiceProvider ConfigureServices()     /*Provedor de servicios*/ 
        {
            var servicios = new ServiceCollection();         

            // Servicios principales
            servicios.AddSingleton<AuthenticationManager>();
            servicios.AddSingleton<TokenManager>();
            servicios.AddSingleton<TokenHander>(); 

            /*servicios de vistas*/
            
            servicios.AddSingleton<FormHost>();
            servicios.AddSingleton<MarkRibbon>();
            servicios.AddTransient<MainWindow>();
            servicios.AddTransient<DrawSignature>();
            servicios.AddTransient<SavePrompt>();
            servicios.AddTransient<SignatureInserter>(); /*clase que inserta la firma*/


          /*servicios de ViewModels*/
            servicios.AddSingleton<MainWindowViewModel>();
            servicios.AddTransient<HomeViewViewModel>();
            servicios.AddTransient<DrawSignatureViewModel>();
            servicios.AddTransient<SavePromptViewModel>();
            
 


            servicios.AddSingleton<INavigationService, MarkAddIn.NavigationService>();
            ;

            return servicios.BuildServiceProvider();

            
        }
    }
}

