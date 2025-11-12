using Mark.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
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


namespace MarkAddIn
{





    public partial class HomeView
    {

        private readonly HomeViewViewModel _HomeViewviewModel;
        public static AuthenticationResult resultado;
        public static JwtSecurityToken jwtToken;
        public static string nombre;            /*hay que darle valor dentro del metodo de aprobar*/
        public static string email;
        public static string objectId;
        private static string firmaRetornadaBytes;
        public static byte[] firmaBytes;




        public HomeView()
        {
            InitializeComponent();
           
            
            _HomeViewviewModel = new HomeViewViewModel();
            this.DataContext = _HomeViewviewModel;

        }

      



        public void ActualizarUsuarioHomeView(string nombre)
        {
            _HomeViewviewModel.HomeViewUsername = nombre ?? "Usuario";
        }






        public void FirmaBtn_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.Windows            /*accede a la coleccion de ventanas en MainWindow*/
                .OfType<MainWindow>().FirstOrDefault();
             
            if (mainWindow != null)   /*si ya se esta proyectando HomeView*/          
            {
                if (mainWindow.DataContext is MainWindowViewModel mainWindowViewModel)
                {
                    var drawSignatureView = new DrawSignature();

                    mainWindowViewModel.CurrentView = drawSignatureView;
                }
            }
        }











        public async void AprobacionBtn_Click(object sender, RoutedEventArgs e)
        {

           
            try
            {
                var servicios = Bootstrapper.ServiceProvider; 

                var apiService = servicios.GetRequiredService<ApiService>();


                objectId = TokenManager.UserObjectId;




                var firma = await apiService.ObtenerFirmaAsync(objectId);                   /*ultimo punto de interrupcion funcional*/


                /*MessageBox.Show(firma.ImgBase64);*/

                var firmaRetornada = firma.ImgBase64;   /*empezar a trabajar con firmaRetornada*/

               /*cambiamos a [] */

                var firmaRetornadaBytes = Convert.FromBase64String(firmaRetornada);



                string tempPath = Path.Combine(Path.GetTempPath(), $"firma_{Guid.NewGuid()}.png");
                File.WriteAllBytes(tempPath, firmaRetornadaBytes);


                firmaBytes = firmaRetornadaBytes;

                SignatureOverture.NombreAutenticado = TokenManager.UserName;

                

            
                SignatureOverture.ActivarInserccionPorClic(firmaBytes);


                var mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                mainWindow?.Close();



                File.Delete(tempPath);
           

                if (firma == null)
                {
                    MessageBox.Show("No se encontró una firma registrada para este usuario.",
                                    "Sin firma", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                MessageBox.Show("Firma obtenida correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}");
            }

           
        }

    }
}
