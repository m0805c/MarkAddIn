
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

        public void Facilitador (AuthenticationResult resultado)
        {
            if (resultado == null)
            {
                throw new InvalidOperationException("No se obtuvo un token.");
            }


            /*guarda el token como variable accessToken*/
            string accessToken = resultado.AccessToken;


            /*Heads Up: sea crea handler dandole una propiedad de System.IdentityModel.Tokens.Jwt. JwtSecurityTokenHandler*/

            var handler = new JwtSecurityTokenHandler();

            /*se lee el token y se crea una variable jwtToken de tipo JwtSecurityToken*/
            var jwtToken = handler.ReadToken(accessToken) as JwtSecurityToken;


           
            }


    }
}
