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
        public string accessToken { get; set; }

        public string UserObjectId { get; set; }



        public void GuardarInfoToken (string AccessToken, string ObjectId)

        {
            AccessToken = accessToken;
            ObjectId = UserObjectId;
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
