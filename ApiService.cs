using MarkAddIn;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


/*revisar las url de cada llamada*/
/*los gets no necesitan manejar DTOs de entrega*/



/*controladores de usuario ya funcionan, ya solo hay que revisar los de firmas*/

namespace Mark.Client
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://localhost:7189/api";/*private readonly string _baseUrl = "https://localhost:7189/api";
*/
        private readonly TokenManager _tokenManager;

        public ApiService(TokenManager tokenManager)
        {
            _tokenManager = tokenManager;
            _httpClient = new HttpClient();
        }

        private void AgregarEncabezados()
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", TokenManager.accessToken);
        }





        /*Request: json -> DTO*, se accede desde el Ribbon para confirmar que se encontró el usuario, 
          no necesita DTO de entrega porque el objectId va en la URL 
        
         Entregamos oId en la URL -> nos devuelve un Json y deserializamos en UsuarioRetornadoDTO*/
        /*entregable: objectIdAzure*/       /*GET/api/Usuarios/{objectIdAzure}*/
        public async Task<UsuarioRetornadoDTO> ObtenerUsuarioAsync(string objectIdAzure)                        /*funciona*/
        {
            AgregarEncabezados();
           var url = $"{_baseUrl}/Usuarios/{objectIdAzure}";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                var detalle = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al obtener usuario: {response.StatusCode}\n{detalle}");
            }
            var contenido = await response.Content.ReadAsStringAsync();
           
            var usuarioRetornado = JsonConvert.DeserializeObject<UsuarioRetornadoDTO>(contenido);

            return usuarioRetornado;


        }



        /*Request: DTO -> Json, entregamos un Json Serializado con Nombre, Email, y oId*/
        /*El POST/api/Usuarios/ recibe un cuerpo Json, el cual extrae el oId y encuentra al usuario*/
        /*entregables: UsuarioPostDTO*/         /*POST/api/Usuarios/{objectIdAzure}*/
         
        public async Task CrearUsuarioAsync(string objectIdAzure, UsuarioPostDTO nuevoUsuario)
        {
            AgregarEncabezados();

            var json = JsonConvert.SerializeObject(nuevoUsuario);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var url = $"{_baseUrl}/Usuarios?ObjectIdAzure={objectIdAzure}"; /*revisar si es correcta la URL*/
            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var detalle = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al crear usuario: {response.StatusCode}\n{detalle}");       
            }

        }

        /*Request: DTO -> Json, entregamos un Json que incluye el oId*/
        /*POST/api/Firmas lo procesa, extrae el oId, lo empata y guarda la firma*/
        /*entregables: FirmaPostDTO y objectIdAzure*/           /*POST/api/Firmas*/
        
        
        /*
        public async Task CrearFirmaAsync (PPFirmaDTO nuevaFirma)
        {
            AgregarEncabezados();
           
            var json = JsonConvert.SerializeObject(nuevaFirma);
            var content = new StringContent (json, Encoding.UTF8, "application/json");

            var url = $"{_baseUrl}/Firmas";                      /*revisar si es correcta la URL
            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var detalle = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al crear firma: {response.StatusCode}\n{detalle}");
            }

        }*/

        



        public async Task GuardarFirmaAsync(PPFirmaDTO firmaDto)
        {
            // Siempre agregar token y encabezados
            AgregarEncabezados();

            var json = JsonConvert.SerializeObject(firmaDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var url = $"{_baseUrl}/Firmas";   

            var response = await _httpClient.PostAsync(url, content);

            // Manejo de error estándar
            if (!response.IsSuccessStatusCode)
            {
                var detalle = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al guardar firma: {response.StatusCode}\n{detalle}");
            }
        }




        /*entegables: objectIdAzure*/               /*GET/api/Firmas/{objectIdAzure}*/
        public async Task<FirmaRetornadaDTO> ObtenerFirmaAsync(string objectIdAzure)
        {
            AgregarEncabezados();

            var url = $"{_baseUrl}/Firmas/{objectIdAzure}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var detalle = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al obtener firma: {response.StatusCode}\n{detalle}");
            }

            var contenido = await response.Content.ReadAsStringAsync();

            MessageBox.Show(contenido);

            // Si la API retorna un solo objeto FirmaRetornadaDTO
            var firmaRetornada = JsonConvert.DeserializeObject<FirmaRetornadaDTO>(contenido);
            return firmaRetornada;
        }
    
      



        /*entregables: FirmaPutDTO y objectIdAzure*/            /*PUT/api/Firmas/{objectIdAzure}*/
        public async Task ActualizarFirmaAsync(string objectIdAzure, PPFirmaDTO firmaActualizada)
        {
            AgregarEncabezados();

            var json = JsonConvert.SerializeObject(firmaActualizada);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var url = $"{_baseUrl}/Firmas?ObjectIdAzure={objectIdAzure}";
            var response = await _httpClient.PutAsync(url, content);


            if (!response.IsSuccessStatusCode)
            {
                var detalle = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al actualizar firma: {response.StatusCode}\n{detalle}");
            }

            }
    }
}



