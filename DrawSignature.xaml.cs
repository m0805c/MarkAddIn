using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
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
using System.Windows.Ink;
using Microsoft.Identity.Client;
using System.IO;
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;
using Mark.Client;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace MarkAddIn
{
    /// <summary>
    /// Lógica de interacción para DrawSignature.xaml
    /// </summary>
    /// 




    public partial class DrawSignature : UserControl
    {
        public readonly DrawSignatureViewModel _drawSignatureViewModel;




         public DrawSignature()
        {
            InitializeComponent();


            _drawSignatureViewModel = new DrawSignatureViewModel();
            this.DataContext = _drawSignatureViewModel;
        }

        
        
        
        /* Clase estática para que se pueda almacenar la firma */
        public static class AlmacenarTrazos
        {
            private static StrokeCollection _signaturestrokes;
            public static StrokeCollection SignatureStrokes
            {
                get => _signaturestrokes;
                set => _signaturestrokes = value;
            }
        }




        public void ActualizarUsuarioDrawSignature(string nombre)
        {
            _drawSignatureViewModel.Username = nombre ?? "Usuario";
        }

        private void BackToHomeBtn_Click(object sender, RoutedEventArgs e)
        {
            var relyWindow = Application.Current.Windows
                .OfType<MainWindow>().FirstOrDefault();

            if (relyWindow != null)
            {
                if (relyWindow.DataContext is MainWindowViewModel mainWindowViewModel)
                {
                    var homeView = new HomeView();
                    mainWindowViewModel.CurrentView = homeView;
                }
            }
        }





        private void EraseFirmaBtn_Click(object sender, RoutedEventArgs e)
        {
            Board.Strokes.Clear();
        }






        public void AcceptFirmaBtn_Click(object sender, RoutedEventArgs e)
        {
            IReadOnlyList<Stroke> strokes = Board.Strokes;

            if (strokes.Count != 0)
            {
                /* Almacenamiento en StrokeCollector */
                AlmacenarTrazos.SignatureStrokes = new StrokeCollection(Board.Strokes);

                using (var Memoria = new MemoryStream())
                {
                    AlmacenarTrazos.SignatureStrokes.Save(Memoria);
                    byte[] firmaEnBytes = Memoria.ToArray();
                    MessageBox.Show($"prueba de captura: {BitConverter.ToString(firmaEnBytes)}",
                                    "Firma en bytes", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                MessageBox.Show("Firma almacenada correctamente.", "Firma guardada", MessageBoxButton.OK, MessageBoxImage.Information);

                Board.Strokes.Clear();






                var relyWindow = Application.Current.Windows
                    .OfType<MainWindow>().FirstOrDefault();

                if (relyWindow != null)
                {
                    if (relyWindow.DataContext is MainWindowViewModel mainWindowViewModel)
                    {
                        var savePrompt = new SavePrompt();
                        mainWindowViewModel.CurrentView = savePrompt;
                    }
                }
            }
            else
            {
                MessageBox.Show("No hay firma dibujada. Por favor, dibuja tu firma antes de avanzar.",
                                "Firma vacía", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            try
            {
                // Puedes manejar excepciones adicionales aquí si es necesario
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error inesperado: " + ex.Message,
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

















