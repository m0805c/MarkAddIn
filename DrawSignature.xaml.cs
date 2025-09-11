using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
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
    /// <summary>
    /// Lógica de interacción para DrawSignature.xaml
    /// </summary>
    
    public partial class DrawSignature : UserControl
    {
        private readonly DrawSignatureViewModel _drawSignatureViewModel;
        public DrawSignature()
        {
            InitializeComponent();
            _drawSignatureViewModel = new DrawSignatureViewModel();
            this.DataContext = _drawSignatureViewModel;
        }
        public void ActualizarUsuarioDrawSignature(string nombre)
        {
            _drawSignatureViewModel.Username = nombre ?? "Usuario";
        }







        private void BackToHomeBtn_Click(object sender, RoutedEventArgs e)
        {
               
        }


        private void EraseFirmaBtn_Click(object sender, RoutedEventArgs e)
        { 
           

        }

        private void AcceptFirmaBtn_Click(object sender, RoutedEventArgs e)
        {
           
        }

      
    }


}

