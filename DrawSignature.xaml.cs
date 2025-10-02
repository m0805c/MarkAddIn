using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MarkAddIn
{
    public partial class DrawSignature : UserControl
    {
        public readonly DrawSignatureViewModel _drawSignatureViewModel;

        // Firma temporal en bytes
        public static class FirmaTemporal
        {
            public static byte[] Bytes { get; set; }
        }

        // Guardar trazos
        public static class AlmacenarTrazos
        {
            private static StrokeCollection _signaturestrokes;
            public static StrokeCollection SignatureStrokes
            {
                get => _signaturestrokes;
                set => _signaturestrokes = value;
            }
        }

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
            var relyWindow = Application.Current.Windows
                .OfType<MainWindow>().FirstOrDefault();
            if (relyWindow?.DataContext is MainWindowViewModel mainWindowViewModel)
            {
                mainWindowViewModel.CurrentView = new HomeView();
            }
        }




        private void EraseFirmaBtn_Click(object sender, RoutedEventArgs e)
        {
            Board.Strokes.Clear();
        }








        public void AcceptFirmaBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Board.Strokes.Count == 0)
            {
                MessageBox.Show("No hay firma dibujada. Por favor, dibuja tu firma antes de avanzar.",
                                "Firma vacía", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                /*captura unicamente los trazos*/
                var strokes = Board.Strokes;
                Rect bounds = strokes.GetBounds();
                double margin = 10;
                bounds.Inflate(margin, margin);

                /*crea un ink canvas temporal solo con los trazos*/
                var tempCanvas = new InkCanvas();
                tempCanvas.Background = Brushes.Transparent;
                tempCanvas.Width = bounds.Width;
                tempCanvas.Height = bounds.Height;

                /* Clonar y mover los trazos para alinearlos a 0,0*/
                var clonedStrokes = strokes.Select(s => s.Clone()).ToList();
                foreach (var s in clonedStrokes)
                {
                    /* Transformación corregida con applyToStylusTip*/
                    s.Transform(new Matrix(1, 0, 0, 1, -bounds.X, -bounds.Y), false);
                    tempCanvas.Strokes.Add(s);
                }

                /*ink canvas a bitmap*/
                var rtb = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, 96, 96, PixelFormats.Pbgra32);
                tempCanvas.Measure(new Size(bounds.Width, bounds.Height));
                tempCanvas.Arrange(new Rect(new Size(bounds.Width, bounds.Height)));
                rtb.Render(tempCanvas);

                /*bitmap a png*/

                byte[] firmaEnBytes;
                using (var ms = new MemoryStream())
                {
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(rtb));
                    encoder.Save(ms);
                    firmaEnBytes = ms.ToArray();
                }

                /*guardar bytes en firma temporal*/
                FirmaTemporal.Bytes = firmaEnBytes;

                MessageBox.Show("Firma capturada y convertida a PNG correctamente.",
                                "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                
                Board.Strokes.Clear();

               
                var relyWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                if (relyWindow?.DataContext is MainWindowViewModel mainWindowViewModel)
                {
                    var savePrompt = new SavePrompt();
                    mainWindowViewModel.CurrentView = savePrompt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al procesar la firma: " + ex.Message,
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}



















