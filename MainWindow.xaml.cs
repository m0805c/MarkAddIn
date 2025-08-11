using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MarkAddIn;
using Mark.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;



namespace MarkAddIn

  
{

   
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class MainWindow : Window

        

    {

      

        public MainWindow()
        {
            InitializeComponent();


            /*aqui inicializo los servicios necesarios para el MainWindow*/
            var servicios = Bootstrapper.ConfigureServices();
            
        }

        
        

        //generado de manera automatica
        private void AprobacionBtn_Click(object sender, RoutedEventArgs e) 
        {

        }

        //generado de manera automatica
        private void FirmaBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    } 
}




        /*

    {

        

        private static readonly string AddInstance = ConfigurationManager.AppSettings["ida:SignIn"];

        private static readonly string AddTenant = ConfigurationManager.AppSettings["ida:Tenant"];

        private static readonly string AddClientId = ConfigurationManager.AppSettings["ida:ClientId"];

        private static readonly string Authority = string.Format
            (AddClientId, AddInstance, AddTenant);


     

        private static readonly string MarkScope =
            ConfigurationManager.AppSettings["Mark:AccessAsUserScope"];     

        private static readonly string Adress =
            ConfigurationManager.AppSettings["Mark:Adress"];

        private static readonly string[] Scopes = { MarkScope };
        private static string MarkAPIAdress
        {
            get
            {
                string baseAdress = MarkAPIAdress;
                return baseAdress.EndsWith("/") ? MarkAPIAdress + "api/Mark"
                                                : MarkAPIAdress + "api/Mark";
            }
        }









    }
}

*/

