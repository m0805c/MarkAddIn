/*clase iniciadora de */

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
            var vista1 = servicios.GetRequiredService<FormHost>();







            try
            {

                var resultado = await authManager.AcquireTokenAsync(forceLogin: true);


                if (resultado != null)          /*si pasa el proceso de autenticación, se extrae info*/
                {
                    string accessToken = resultado.AccessToken;
                    var handler = new JwtSecurityTokenHandler();

                    var jwtToken = handler.ReadToken(accessToken) as JwtSecurityToken;

                    /*ahora si hay que leer la info*/



                    string objectId = jwtToken?.Claims.FirstOrDefault(c => c.Type == "oid")?.Value;
                    string expiracion = resultado.ExpiresOn.ToString();
                    string cuenta = resultado.Account?.Username;
                    string scope = resultado.Scopes.FirstOrDefault();
                    string nombre = jwtToken?.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
                    string tenantId = jwtToken?.Claims.FirstOrDefault(c => c.Type == "tid")?.Value;

                    MessageBox.Show($"Token expirará el: {expiracion}\nUsuario: {cuenta} \nObjectId: {objectId} \nScope:{scope} \nNombre: {nombre} \nTenant: {tenantId}");

                    /*aqui podria ser que se guarden las funciones*/

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
                if (vista1.InvokeRequired)
                {
                    vista1.Invoke(new Action(() => vista1.Show()));
                }
                else
                {
                    vista1.Show();
                }
            }
        }
    }
}




