
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mark.Client;
using Microsoft.Identity.Client;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace MarkAddIn
{

    public class TokenHander

    {

        public string Nombre { get; private set; }


        public void Facilitador(AuthenticationResult resultado)
        {
            if (resultado == null)
            {
                throw new InvalidOperationException("No se obtuvo un token."); 
            }


            string accessToken = resultado.AccessToken;

            var handler = new JwtSecurityTokenHandler();

            var jwtToken = handler.ReadToken(accessToken) as JwtSecurityToken;


            /*este es el objeto que maneja el nombre*/

             Nombre = jwtToken?.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
               

            }
        }
    }
