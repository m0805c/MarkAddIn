
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

namespace MarkAddIn
{

    
   
    public class TokenHander

    {

        public void Facilitador(AuthenticationResult resultado)
        {
            if (resultado == null)
            {
                throw new InvalidOperationException("No se obtuvo un token.");
            }


            string accessToken = resultado.AccessToken;

            var handler = new JwtSecurityTokenHandler();

            var jwtToken = handler.ReadToken(accessToken) as JwtSecurityToken;

            string nombre = jwtToken?.Claims.FirstOrDefault(c => c.Type == "name")?.Value;


            TokenManager.UserName = nombre;
           
            /*string objectId = jwtToken?.Claims.FirstOrDefault(c => c.Type == "oid")?.Value;
            string expiracion = resultado.ExpiresOn.ToString();
            string cuenta = resultado.Account?.Username;
            string scope = resultado.Scopes.FirstOrDefault();
            string token = resultado.AccessToken;
            string tenantId = jwtToken?.Claims.FirstOrDefault(c => c.Type == "tid")?.Value;*/


        }


    }



}

