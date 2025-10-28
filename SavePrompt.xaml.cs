using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Core;

namespace MarkAddIn
{
    public partial class SavePrompt : UserControl
    {
        public SavePrompt()
        {
            InitializeComponent();

            MostrarPrevisualizacion();
        }

        private void MostrarPrevisualizacion()
        {
            if (DrawSignature.FirmaTemporal.Bytes != null)
            {
                using var ms = new MemoryStream(DrawSignature.FirmaTemporal.Bytes);
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = ms;
                bitmap.EndInit();

                /*ajustar la imagen sin recortar con el uniform*/
                PreviewImage.Source = bitmap;
                PreviewImage.Stretch = System.Windows.Media.Stretch.Uniform;
                PreviewImage.HorizontalAlignment = HorizontalAlignment.Center;
                PreviewImage.VerticalAlignment = VerticalAlignment.Center;


                double maxWidth = 400;
                double maxHeight = 200;
                PreviewImage.MaxWidth = maxWidth;
                PreviewImage.MaxHeight = maxHeight;
            }
        }




        private void NoGuardarBtn_Click(object sender, RoutedEventArgs e)
        {

            SignatureInserter.InsertarFirmaFlotante(
                DrawSignature.FirmaTemporal.Bytes


            );

            MessageBox.Show("Firma insertada bien.",
                            "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

            var mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            mainWindow?.Close();
        }



        private void GuardarBtn_Click(object sender, RoutedEventArgs e)
        {



        }
    }
}































































































