using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Core;
using Microsoft.Identity.Client;
using Mark.Client;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.IdentityModel.Selectors;

namespace MarkAddIn
{



    public partial class SavePrompt : UserControl
    {

        public static string OidReferencia;

        /*crear instancias para referenciar el oid*/





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

            SignatureOverture.NombreAutenticado = TokenManager.UserName;

            SignatureOverture.ActivarInserccionPorClic(DrawSignature.FirmaTemporal.Bytes);

            var mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            mainWindow?.Close();
        }



        public async void GuardarBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var servicios = Bootstrapper.ServiceProvider;
                var apiService = servicios.GetRequiredService<ApiService>();

               
                var usuarioInfo = await apiService.ObtenerUsuarioAsync(TokenManager.UserObjectId);

              
                var PPFirma = new PPFirmaDTO
                {
                    ImgBase64 = Convert.ToBase64String(DrawSignature.FirmaTemporal.Bytes),
                    ObjectIdAzure = usuarioInfo.ObjectIdAzure
                };

                /*en vez de que el cliente determine, solo le da la opcion de "guardarla", el servidor determina si se guarda o no*/
                await apiService.GuardarFirmaAsync(PPFirma);

            
                SignatureOverture.NombreAutenticado = TokenManager.UserName;
                SignatureOverture.ActivarInserccionPorClic(DrawSignature.FirmaTemporal.Bytes);

       
                var mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                mainWindow?.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la firma: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}











/*
SignatureOverture.NombreAutenticado = TokenManager.UserName;
                SignatureOverture.ActivarInserccionPorClic(DrawSignature.FirmaTemporal.Bytes);
                var mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                mainWindow?.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la firma: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
*/






/* try   


                 {
                     var servicios = Bootstrapper.ServiceProvider;
                     var apiService = servicios.GetRequiredService<ApiService>();



                     var NuevaFirma = new PPFirmaDTO
                     {
                         ImgBase64 = Convert.ToBase64String(DrawSignature.FirmaTemporal.Bytes),
                        ObjectIdAzure = TokenManager.UserObjectId,
                     };



                    await apiService.CrearFirmaAsync(NuevaFirma);

                   */























































































































































































