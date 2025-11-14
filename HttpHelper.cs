using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MarkAddIn
{
    public class HttpHelper
    {
        static readonly HttpClient client = new HttpClient();

        static async Task Main()
        {
            try 


            {

            
                using HttpResponseMessage respuesta = await client.GetAsync("https://api.github.com/repos/dotnet/AspNetCore.Docs/branches/main");


                respuesta.EnsureSuccessStatusCode();
                string cuerpodeRespuesta = await respuesta.Content.ReadAsStringAsync();
                
                Console.WriteLine(cuerpodeRespuesta);
            }

            catch(HttpRequestException e)

            {
               
                Console.WriteLine("Message :{0} ", e.Message);
            }

        }
    }
}
