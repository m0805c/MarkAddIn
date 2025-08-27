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

            /*servicios de vista o! 🔹 
             * 
             * 
             * 
             * Entonces el siguiente paso lógico es crear los UserControls DrawSignatureView y ApprovalView (antes llamados DrawSignature.xaml y tu layout de aprobación), y sus ViewModels correspondientes.*/

            servicios.AddSingleton<FormHost>();
            servicios.AddSingleton<MarkRibbon>();
            servicios.AddTransient<MainWindow>();
            servicios.AddTransient<DrawSignature>();


            /*ViewModels*/
            servicios.AddSingleton<MainWindowViewModel>();
            //servicios.AddTransient<DrawSignaturViewModel>();
            //ApprovalViewModel

            servicios.AddSingleton<INavigationService, MarkAddIn.NavigationService>();

            return servicios.BuildServiceProvider();

            
        }
    }
}

