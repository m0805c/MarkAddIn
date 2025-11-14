using Mark.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Windows;
using System.Windows.Controls;
using System.Configuration;
using System.Windows.Navigation;
using System;





namespace MarkAddIn

{



    public partial class MainWindow : Window
    {
    
        private readonly MainWindowViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            
            _viewModel = new MainWindowViewModel();
            this.DataContext = _viewModel;
            
        }

        public void ActualizarUsuario(string nombre)            

        {
            _viewModel.Username = nombre ?? "Usuario";
        }

    }

}
















