

using Mark.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Microsoft.Office.Tools;
using Microsoft.Office.Tools.Ribbon;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Windows.Forms;

namespace MarkAddIn
{
    public partial class MarkRibbon
    {
        private void MarkRibbon_Load(object sender, RibbonUIEventArgs e)
        {


        }

        private async void button1_Click(object sender, RibbonControlEventArgs e)
        {

            /*carga de dependencias*/

            var servicios = Bootstrapper.ConfigureServices();
            var authManager = servicios.GetRequiredService<AuthenticationManager>();
            var tokenHandler = servicios.GetRequiredService<TokenHander>();
            var tokenManager = servicios.GetRequiredService<TokenManager>();
            

   
            var mainView = servicios.GetRequiredService<MainWindow>();
            var DrawSignature = servicios.GetRequiredService<DrawSignature>();

            try
            {

                var resultado = await authManager.AcquireTokenAsync(forceLogin: true);


                if (resultado != null)          
                {
                    


            

                    var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(resultado.AccessToken);

                    string nombre = jwtToken?.Claims.FirstOrDefault(c => c.Type == "name")?.Value;

                    
                    MessageBox.Show($"\nNombre: {nombre}");

                    TokenManager.UserName = nombre;



                    mainView.ActualizarUsuario(nombre);


             





                }

                else

                {
                    MessageBox.Show("primer catchup.");
                }


            }

            catch (Exception error)

            {
                MessageBox.Show($"aqui es el error: {error.Message}");
            }

            finally

            {
                if (mainView.Dispatcher.CheckAccess())
                {


                   
                    mainView.Show();

                    /*mainView.Invoke(new Action(() => mainView.Show()));*/
                }
                else
                {
                    mainView.Dispatcher.Invoke(() => mainView.Show());
                }
            }
        }
    }
}




