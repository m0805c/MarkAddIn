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




        public async Task<UsuarioRetornadoDTO> ObtenerUsuarioAsync(string objectIdAzure)                       
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



        public async Task CrearUsuarioAsync(string objectIdAzure, UsuarioPostDTO nuevoUsuario)
        {
            AgregarEncabezados();

            var json = JsonConvert.SerializeObject(nuevoUsuario);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var url = $"{_baseUrl}/Usuarios?ObjectIdAzure={objectIdAzure}"; 
            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var detalle = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al crear usuario: {response.StatusCode}\n{detalle}");       
            }

        }

  
        public async Task GuardarFirmaAsync(PPFirmaDTO firmaDto)
        {
         
            AgregarEncabezados();

            var json = JsonConvert.SerializeObject(firmaDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var url = $"{_baseUrl}/Firmas";   

            var response = await _httpClient.PostAsync(url, content);

       
            if (!response.IsSuccessStatusCode)
            {
                var detalle = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al guardar firma: {response.StatusCode}\n{detalle}");
            }
        }



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

         
            var firmaRetornada = JsonConvert.DeserializeObject<FirmaRetornadaDTO>(contenido);
            return firmaRetornada;
        }
    
      
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



