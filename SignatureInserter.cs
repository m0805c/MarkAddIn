using System;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Core;
using Microsoft.Identity.Client;
using System.Windows.Documents;

namespace MarkAddIn
{
    public static class SignatureInserter
    {

        public static void InsertarFirmaFlotante(byte[] firmaBytes)
        {
            if (firmaBytes == null || firmaBytes.Length == 0)
                throw new ArgumentException("La firma está vacía o no se generó correctamente.");

            string tempPath = Path.Combine(Path.GetTempPath(), $"firma_{Guid.NewGuid()}.png");
            File.WriteAllBytes(tempPath, firmaBytes);

            var app = Globals.ThisAddIn.Application;
            Excel.Worksheet hojaActiva = (Excel.Worksheet)app.ActiveSheet;

                    hojaActiva.Unprotect();

            Excel.Range rangoPrincipal = hojaActiva.Range["B2", "D4"];
            Excel.Range rangoAlternativo = hojaActiva.Range["E2", "G4"];

            // Verificar si ya hay una imagen en el rango principal
            bool rangoOcupado = false;
            float leftPrincipal = (float)(double)rangoPrincipal.Left;
            float topPrincipal = (float)(double)rangoPrincipal.Top;
            float rightPrincipal = leftPrincipal + (float)(double)rangoPrincipal.Width;
            float bottomPrincipal = topPrincipal + (float)(double)rangoPrincipal.Height;

            foreach (Excel.Shape shape in hojaActiva.Shapes)
            {
                float shapeLeft = shape.Left;
                float shapeTop = shape.Top;
                float shapeRight = shapeLeft + shape.Width;
                float shapeBottom = shapeTop + shape.Height;

                // Verifica si la imagen está dentro del rango principal
                if (shapeLeft >= leftPrincipal && shapeRight <= rightPrincipal &&
                    shapeTop >= topPrincipal && shapeBottom <= bottomPrincipal)
                {
                    rangoOcupado = true;
                    break;
                }
            }

            Excel.Range rangoDestino = rangoOcupado ? rangoAlternativo : rangoPrincipal;

            float left = (float)(double)rangoDestino.Left;
            float top = (float)(double)rangoDestino.Top;
            float width = (float)(double)rangoDestino.Width;
            float height = (float)(double)rangoDestino.Height;

            var FirmaMarcaDeAgua = hojaActiva.Shapes.AddPicture(
                Filename: tempPath,
                LinkToFile: MsoTriState.msoFalse,
                SaveWithDocument: MsoTriState.msoTrue,
                Left: left,
                Top: top,
                Width: width,
                Height: height
            );

            FirmaMarcaDeAgua.LockAspectRatio = MsoTriState.msoTrue;
            FirmaMarcaDeAgua.Placement = Excel.XlPlacement.xlFreeFloating;

            hojaActiva.Protect(Contents: true, DrawingObjects: true);

            File.Delete(tempPath);
        }

        
       
    }
}