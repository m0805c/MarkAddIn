using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using  System.Configuration;
using System.Globalization;
using Microsoft.Identity.Client;
using Mark.Client;


namespace MarkAddIn
{


    
    public class TokenManager
   
    {

        
        public static string accessToken { get; set; }

        public static string UserObjectId { get; set;}
        public static string UserName { get; set; }


       


        public void GuardarInfoToken (string AccessToken, string ObjectId, string NombreUsuario)

        {
            AccessToken = accessToken;
            ObjectId = UserObjectId;
            NombreUsuario = UserName;


            //NombreUsuario = nombre;
        }

        public void CleanUp ()
                    {
            accessToken = null;
            UserObjectId = null;
            
        }

        public bool HasValidToken() => !string.IsNullOrEmpty(accessToken) && 
            !string.IsNullOrEmpty(UserObjectId);

        
    }

}
