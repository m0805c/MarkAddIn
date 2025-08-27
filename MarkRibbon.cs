

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
            var tokenManager =
                servicios.GetRequiredService<TokenManager>();

            /*carga de UI*/

            var mainView = servicios.GetRequiredService<MainWindow>();

            try
            {

                var resultado = await authManager.AcquireTokenAsync(forceLogin: true);


                if (resultado != null)          /*si pasa el proceso de autenticación, se extrae info*/
                {



                    /*se cambió la estructura para hacer una referencia directa desde el tokenhander.cs*/


                    /*esto no es necesario replicarlo, ya que las unico contenido que va a cambiar es el contenido central*/


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




