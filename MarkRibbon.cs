using Mark.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using Microsoft.Office.Tools.Ribbon;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace MarkAddIn
{
    public partial class MarkRibbon
    {
        public static AuthenticationResult resultado;
        public static JwtSecurityToken jwtToken;
        public static string nombre; 
        public static string email;
        public static string objectId;

        private void MarkRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            // Inicialización del Ribbon
        }

        private async void button1_Click(object sender, RibbonControlEventArgs e)
        {
            // Carga de dependencias
            var servicios = Bootstrapper.ConfigureServices();

            var authManager = servicios.GetRequiredService<AuthenticationManager>();
            var tokenManager = servicios.GetRequiredService<TokenManager>();
            var apiService = servicios.GetRequiredService<ApiService>();
            var mainView = servicios.GetRequiredService<MainWindow>();

            try
            {
              
                 resultado = await authManager.AcquireTokenAsync(forceLogin: true);

                if (resultado == null)
                {
                    MessageBox.Show("Error: no se pudo obtener el token de acceso.");
                    return;
                }

                
                 jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(resultado.AccessToken);

                 nombre = jwtToken?.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
                 email = jwtToken?.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value;
                 objectId = jwtToken?.Claims.FirstOrDefault(c => c.Type == "oid")?.Value;

                if (string.IsNullOrEmpty(objectId))
                {
                    MessageBox.Show("Error: no se pudo obtener el ObjectId del usuario.");
                    return;
                }


                
                TokenManager.UserName = nombre;
                TokenManager.accessToken = resultado.AccessToken;
                TokenManager.UserObjectId = objectId;

               
                var nuevoUsuario = new UsuarioPostDTO
                {
                    Nombre = nombre,
                    Email = email,
                    ObjectIdAzure = objectId
                };

               
                try
                {
                    var usuarioExistente = await apiService.ObtenerUsuarioAsync(objectId);



                    /*se va a usar .oId para sacar la firma y con el vamos a trabajar las solicitudes, primero put y luegi post*/


                    MessageBox.Show(usuarioExistente.Nombre, usuarioExistente.ObjectIdAzure);
                    


                    if (usuarioExistente != null)
                    {
                        MessageBox.Show($"Bienvenido, {nombre}.");
                    }
                    else
                    {
                      
                        

                        await apiService.CrearUsuarioAsync(objectId, nuevoUsuario);
                        MessageBox.Show($"El usuario de {nombre} creado correctamente.");
                    }
                }
                catch (Exception ex)
                {
                   
                    if (ex.Message.Contains("404"))
                    {
                        try
                        {
                            await apiService.CrearUsuarioAsync(objectId, nuevoUsuario);
                            MessageBox.Show($"Usuario {nombre} no existía y fue creado correctamente.");
                        }
                        catch (Exception createEx)
                        {
                            MessageBox.Show($"Error al crear usuario: {createEx.Message}");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Error al verificar usuario: {ex.Message}");
                        return;
                    }
                }

                
                mainView.ActualizarUsuario(nombre);

                if (mainView.Dispatcher.CheckAccess())
                    mainView.Show();
                else
                    mainView.Dispatcher.Invoke(() => mainView.Show());
            }
            catch (Exception error)
            {
                MessageBox.Show($"Error general durante el inicio de sesión: {error.Message}");
            }
        }
    }
}





