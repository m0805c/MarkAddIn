
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

        public string Nombre { get; set; }
     
        public static JwtSecurityTokenHandler handler { get; set; }
        public static JwtSecurityToken jwtToken { get; set; }



        public void Facilitador(AuthenticationResult resultado)
        {
            if (resultado == null)
            {
                throw new InvalidOperationException("No se obtuvo un token.");
            }

            string accessToken = resultado.AccessToken;

     
            handler = new JwtSecurityTokenHandler();

        
            jwtToken = handler.ReadToken(accessToken) as JwtSecurityToken;

            Nombre = jwtToken?.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
        }
        }
    }
