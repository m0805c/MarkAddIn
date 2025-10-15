
using System;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Core;

namespace MarkAddIn
{
    public static class SignatureInserter
    {
        public static void InsertarFirmaFlotante(byte[] firmaBytes, float left = 100f, float top = 100f, float width = 250f, float height = 100f)
        {
            if (firmaBytes == null || firmaBytes.Length == 0)
                throw new ArgumentException("La firma está vacía o no se generó correctamente.");

            
            string tempPath = Path.Combine(Path.GetTempPath(), $"firma_{Guid.NewGuid()}.png");
            File.WriteAllBytes(tempPath, firmaBytes);

            /*modelo de acceso*/
            var app = Globals.ThisAddIn.Application;
            Excel.Worksheet hojaActiva = (Excel.Worksheet)app.ActiveSheet;

            
            var shape = hojaActiva.Shapes.AddPicture(
                Filename: tempPath,
                LinkToFile: MsoTriState.msoFalse,
                SaveWithDocument: MsoTriState.msoTrue,
                Left: left,       
                Top: top,         
                Width: width,     
                Height: height   
            );

            
            shape.LockAspectRatio = MsoTriState.msoTrue;
            shape.Placement = Excel.XlPlacement.xlFreeFloating;
           

            // Puedes opcionalmente proteger la hoja para que no se mueva
            // hojaActiva.Protect(Contents: true, DrawingObjects: true);

            // Borrar el archivo temporal
            File.Delete(tempPath);
        }
    }
}