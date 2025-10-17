using System;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Core;
using Microsoft.Identity.Client;
using System.Windows.Documents;
using System.Windows.Media.Animation;

namespace MarkAddIn
{
    public static class SignatureInserter
    {
        /*clase para insetar la firma*/
        public static void InsertarFirmaFlotante(byte[] firmaBytes)
        {
            if (firmaBytes == null || firmaBytes.Length == 0)
                throw new ArgumentException("La firma está vacía o no se generó correctamente.");

            string tempPath = Path.Combine(Path.GetTempPath(), $"firma_{Guid.NewGuid()}.png");
            File.WriteAllBytes(tempPath, firmaBytes);

            var app = Globals.ThisAddIn.Application;
            Excel.Worksheet hojaActiva = (Excel.Worksheet)app.ActiveSheet;

            hojaActiva.Unprotect();

         
            Excel.Range[] rangos = new Excel.Range[]
            {
                hojaActiva.Range["C48", "D50"], 
                hojaActiva.Range["C56", "D58"], 
                hojaActiva.Range["H48", "I50"],
                hojaActiva.Range["H56", "I58"]  
            };

            Excel.Range rangoSeleccionado = null;

            // Buscar el primer rango disponible
            foreach (var rango in rangos)
            {
                float left = (float)(double)rango.Left;
                float top = (float)(double)rango.Top;
                float right = left + (float)(double)rango.Width;
                float bottom = top + (float)(double)rango.Height;

                bool ocupado = false;
                foreach (Excel.Shape shape in hojaActiva.Shapes)
                {
                    float shapeLeft = shape.Left;
                    float shapeTop = shape.Top;
                    float shapeRight = shapeLeft + shape.Width;
                    float shapeBottom = shapeTop + shape.Height;

                    if (shapeLeft >= left && shapeRight <= right &&
                        shapeTop >= top && shapeBottom <= bottom)
                    {
                        ocupado = true;
                        break;
                    }
                }

                if (!ocupado)
                {
                    rangoSeleccionado = rango;
                    break;
                }
            }

            // Si todos los rangos están ocupados, usar el último
            if (rangoSeleccionado == null)
                rangoSeleccionado = rangos[rangos.Length - 1];

            float impresionLeft = (float)(double)rangoSeleccionado.Left;
            float impresionTop = (float)(double)rangoSeleccionado.Top;
            float impresionWidth = (float)(double)rangoSeleccionado.Width;
            float impresionHeight = (float)(double)rangoSeleccionado.Height;

            var FirmaMarcaDeAgua = hojaActiva.Shapes.AddPicture(
                Filename: tempPath,
                LinkToFile: MsoTriState.msoFalse,
                SaveWithDocument: MsoTriState.msoTrue,
                Left: impresionLeft,
                Top: impresionTop,
                Width: impresionWidth,
                Height: impresionHeight
            );

            FirmaMarcaDeAgua.LockAspectRatio = MsoTriState.msoTrue;
            FirmaMarcaDeAgua.Placement = Excel.XlPlacement.xlFreeFloating;



            hojaActiva.Protect(Contents: true, DrawingObjects: true);

            File.Delete(tempPath);


  
                
            
            
        }


        

       
    }
}