using Mark.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Windows;





namespace MarkAddIn

  
{


   

   
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class MainWindow : Window

    {
        public MainWindow()
        {
            var servicios = Bootstrapper.ConfigureServices();
            var authenticationManager = servicios.GetRequiredService<AuthenticationManager>();
            var tokenHandler = servicios.GetRequiredService<TokenHander>();


            InitializeComponent();
            
        }


        
        private void AprobacionBtn_Click(object sender, RoutedEventArgs e) 
        {

        }

        
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

