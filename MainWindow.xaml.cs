using Mark.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Windows;
using System.Windows.Controls;
using System.Configuration;
using System.Windows.Navigation;
using System;





namespace MarkAddIn

{



    public partial class MainWindow : Window
    {
    
        private readonly MainWindowViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            
            _viewModel = new MainWindowViewModel();
            this.DataContext = _viewModel;
            
        }

        public void ActualizarUsuario(string nombre)            

        {
            _viewModel.Username = nombre ?? "Usuario";
        }

    }

}























        /*
            InitializeComponent();

          

         
            

            MessageBox.Show($"prueba: '{TokenManager.UserName ?? "NULL"}'");


         

        }

        public void ActualizarUsuario (string nombre)
        {
                       UsuarioLabel.Content = nombre;
        }


       
    } 
}
*/



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

