

using MarkAddIn;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Cache;
using Microsoft.Identity.Client.Extensions.Msal;
using Microsoft.Office.Core;
using System;
using System.Configuration;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Mark;



namespace Mark.Client
{

    public class AuthenticationManager : Window
    {
        // se leé la información de la aplicación desde el archivo de configuración

        private readonly IPublicClientApplication _mark;
        private readonly string[] _scopes;

        public AuthenticationResult ultimaAutenticacion { get; private set; }
        public string AccessToken => ultimaAutenticacion?.AccessToken;
        public string ObjectId => ultimaAutenticacion?.Account?.HomeAccountId?.ObjectId;

        // representante de la aplicación
        public AuthenticationManager()
        {
            var aadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
            var tenant = ConfigurationManager.AppSettings["ida:Tenant"];
            var IdCliente = ConfigurationManager.AppSettings["ida:ClientId"];
            var conAutoridad = string.Format(CultureInfo.InvariantCulture, aadInstance, tenant);
            _scopes = new[] { ConfigurationManager.AppSettings["Mark:AccessAsUserScope"] };

            

            //ya no es necesario inicializarse desde el app.xaml.cs

            _mark = PublicClientApplicationBuilder.Create(IdCliente)
                .WithAuthority(conAutoridad)
                .WithRedirectUri("http://localhost")
                .Build();


            // variable para la creación de propiedades de almacenamiento

            var storageProperties = new StorageCreationPropertiesBuilder(
                        "MarkCache",
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                        .Build();

            // variable para la creación de almacenador de credenciales de caché

            var cacheHelper = MsalCacheHelper.CreateAsync(storageProperties).GetAwaiter().GetResult();
            cacheHelper.RegisterCache(_mark.UserTokenCache);
        }


        //prueba con bool false al 04/08

        
        public async Task<AuthenticationResult> AcquireTokenAsync(bool forceLogin = false)
        {

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var cuentas = (await _mark.GetAccountsAsync()).ToList();
            if (!forceLogin && cuentas.Any())
            {
                try
                {
                    ultimaAutenticacion = await _mark
                        .AcquireTokenSilent(_scopes, cuentas.First())
                        .ExecuteAsync()
                        .ConfigureAwait(false);

                 
                    return ultimaAutenticacion;
                }
                catch (MsalUiRequiredException)
                {
                   
                }
            }



            var interactiveBuilder = _mark.AcquireTokenInteractive(_scopes);
            interactiveBuilder = forceLogin
                ? interactiveBuilder.WithPrompt(Prompt.ForceLogin)
                : interactiveBuilder.WithPrompt(Prompt.SelectAccount);


            if (_mark.IsEmbeddedWebViewAvailable())
                interactiveBuilder = interactiveBuilder.WithUseEmbeddedWebView(true);
            else
                interactiveBuilder = interactiveBuilder.WithUseEmbeddedWebView(false);

            ultimaAutenticacion = await interactiveBuilder.ExecuteAsync().ConfigureAwait(false);


            var tokenHandler = new TokenHander();
            tokenHandler.Facilitador(ultimaAutenticacion);

            return ultimaAutenticacion;



        }

        // metodo para cerrar sesión 
        public async Task SignOutAsync()
        {
            var cuentas = await _mark.GetAccountsAsync();
            foreach (var acc in cuentas)
                await _mark.RemoveAsync(acc);
        }



        

    }
      
    }





