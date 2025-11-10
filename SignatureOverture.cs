using Mark.Client;
using Microsoft.Office.Core;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;

namespace MarkAddIn
{
    public class SignatureOverture
    {
        public static string NombreAutenticado;
        public static string tempPathName;
        public static string tempPath;

        public static Excel.Application app = Globals.ThisAddIn.Application;

        public static void ActivarInserccionPorClic(byte[] firmaBytes)
        {
            if (string.IsNullOrWhiteSpace(NombreAutenticado))
                throw new ArgumentException("El nombre está vacío.");

            if (firmaBytes == null || firmaBytes.Length == 0)
                throw new ArgumentException("Firma vacía.");

            // Guardar firma temporal
            tempPath = Path.Combine(Path.GetTempPath(), $"firma_{Guid.NewGuid()}.png");
            File.WriteAllBytes(tempPath, firmaBytes);

            // Guardar nombre temporal como imagen
            tempPathName = Path.Combine(Path.GetTempPath(), $"Nombre_{Guid.NewGuid()}.png");
            RenderTextToPng(tempPathName);

            // Desbloquea la hoja
            var hojaActiva = (Excel.Worksheet)app.ActiveSheet;
            hojaActiva.Unprotect("P5w0soA314641z");

            // Enganchar evento
            app.SheetBeforeDoubleClick += OnSheetBeforeDoubleClick;
            MessageBox.Show("Doble clic donde deseas insertar la firma.");
        }

        private static void RenderTextToPng(string outputPath)
        {
            int width = 350, height = 80;

            using Bitmap bitmap = new(width, height);
            using Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);
            using Font font = new("Segoe UI", 16);
            using Brush brush = new SolidBrush(Color.Black);

            // Medir el tamaño del texto
            SizeF textSize = graphics.MeasureString(NombreAutenticado, font);

            // Calcular posición centrada
            float x = (width - textSize.Width) / 2;
            float y = (height - textSize.Height) / 2;

            graphics.DrawString(NombreAutenticado, font, brush, new PointF(x, y));
            bitmap.Save(outputPath, ImageFormat.Png);
        }

        public static void OnSheetBeforeDoubleClick(object Sh, Excel.Range Target, ref bool Cancel)
        {
            Cancel = true;

            
            app.SheetBeforeDoubleClick -= OnSheetBeforeDoubleClick;

            try
            {
                Excel.Worksheet hoja = (Excel.Worksheet)Sh;

                InsertarFirmaEnCelda(hoja, Target);
                InsertarNombreAutenticado(hoja, Target);

                MessageBox.Show("Firma insertada correctamente.");
            }
            catch (Exception ex)
            {
                {
                    MessageBox.Show("Error al insertar la firma: " + ex.Message);
                }
            }

            finally
            {
                FinalizarInserccion();
            }
        }

        public static void InsertarFirmaEnCelda(Excel.Worksheet hoja, Excel.Range celda)
        {
            var shape = hoja.Shapes.AddPicture(
                tempPath,
                MsoTriState.msoFalse,
                MsoTriState.msoTrue,
                (float)celda.Left, (float)celda.Top,
                160f, 60f
            );

            shape.LockAspectRatio = MsoTriState.msoFalse;
            shape.Placement = Excel.XlPlacement.xlMoveAndSize;
            shape.Locked = true;

            
        }

        public static void InsertarNombreAutenticado(Excel.Worksheet hoja, Excel.Range celda)
        {
            var shape = hoja.Shapes.AddPicture(
                tempPathName,
                MsoTriState.msoFalse,
                MsoTriState.msoTrue,
                (float)celda.Left, (float)celda.Top + 75f,
                200f, 40f
            );

            shape.LockAspectRatio = MsoTriState.msoFalse;
            shape.Placement = Excel.XlPlacement.xlMoveAndSize;
            shape.Locked = true;

            hoja.Protect("P5w0soA314641z", DrawingObjects: true, Contents: true);
        }

        public static void FinalizarInserccion()
        {
            try
            {
                File.Delete(tempPath);
                File.Delete(tempPathName);
            }
            catch { }
        }
    }
}

