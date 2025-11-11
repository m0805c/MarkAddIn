using MarkAddIn;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Office.Interop.Excel;
using System;

namespace Mark.Client
{
    public static class Bootstrapper
    {
        private static IServiceProvider _serviceProvider;

        public static IServiceProvider ConfigureServices()
        {
            if (_serviceProvider != null)
                return _serviceProvider;

            var servicios = new ServiceCollection();

            // Servicios principales
            servicios.AddSingleton<AuthenticationManager>();
            servicios.AddSingleton<TokenManager>();
            servicios.AddSingleton<TokenHander>();
            servicios.AddSingleton<ApiService>(); // servicio registrado

            // Servicios de vistas
            servicios.AddSingleton<FormHost>();
            servicios.AddSingleton<MarkRibbon>();
            servicios.AddTransient<MainWindow>();
            servicios.AddTransient<DrawSignature>();
            servicios.AddTransient<SavePrompt>();
            servicios.AddTransient<SignatureInserter>(); // clase que inserta la firma

            // ViewModels
            servicios.AddSingleton<MainWindowViewModel>();
            servicios.AddTransient<HomeViewViewModel>();
            servicios.AddTransient<DrawSignatureViewModel>();
            servicios.AddTransient<SavePromptViewModel>();

            // Servicios de navegación
            servicios.AddSingleton<INavigationService, MarkAddIn.NavigationService>();

            _serviceProvider = servicios.BuildServiceProvider();
            return _serviceProvider;
        }

        public static IServiceProvider ServiceProvider => _serviceProvider ?? ConfigureServices();
    }
}
