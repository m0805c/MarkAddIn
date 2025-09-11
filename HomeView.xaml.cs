using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MarkAddIn
{





    public partial class HomeView
    {

        private readonly HomeViewViewModel _HomeViewviewModel;
        public HomeView()
        {
            InitializeComponent();
           
            
            _HomeViewviewModel = new HomeViewViewModel();
            this.DataContext = _HomeViewviewModel;

        }

      



        public void ActualizarUsuarioHomeView(string nombre)
        {
            _HomeViewviewModel.HomeViewUsername = nombre ?? "Usuario";
        }






        public void FirmaBtn_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.Windows            /*accede a la coleccion de ventanas en MainWindow*/
                .OfType<MainWindow>().FirstOrDefault();
             
            if (mainWindow != null)   /*si ya se esta proyectando HomeView*/          
            {
                if (mainWindow.DataContext is MainWindowViewModel mainWindowViewModel)
                {
                    var drawSignatureView = new DrawSignature();

                    mainWindowViewModel.CurrentView = drawSignatureView;
                }
            }
        }





        
      



        
        private void AprobacionBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
