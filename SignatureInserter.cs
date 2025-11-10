using System;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Core;
using Microsoft.Identity.Client;
using System.Windows.Documents;
using System.Windows.Media.Animation;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;

namespace MarkAddIn
{
    public class SignatureInserter
    {
        /*clase para insetar la firma*/
        public static void InsertarFirmaFlotante(byte[] firmaBytes)
        {


            /*define la firma*/


            if (firmaBytes == null || firmaBytes.Length == 0)
                throw new ArgumentException("La firma está vacía o no se generó correctamente.");
            /*define el string path pero dentro del metodo*/

            string tempPath = Path.Combine(Path.GetTempPath(), $"firma_{Guid.NewGuid()}.png");
            File.WriteAllBytes(tempPath, firmaBytes);


           /*define la aplicacion*/
            var app = Globals.ThisAddIn.Application;
            Excel.Worksheet hojaActiva = (Excel.Worksheet)app.ActiveSheet;

            var appWorkbook = Globals.ThisAddIn.Application;
            Excel.Workbook libroActivo = appWorkbook.ActiveWorkbook;
            hojaActiva.Unprotect(Password: "P5w0soA314641z");
         


            
           static void EliminarConfigRangosParaCeldas()


                
            {
                var appRanges = Globals.ThisAddIn.Application;
                Excel.Worksheet rangosHojaActiva = (Excel.Worksheet)appRanges.ActiveSheet;

                Excel.AllowEditRanges rangosEdicion = rangosHojaActiva.Protection.AllowEditRanges;

                try
                {
                   
                    for (int i = rangosEdicion.Count; i >= 1; i--)
                    {
                        Excel.AllowEditRange rango = rangosEdicion[i];
                        rango.Delete();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar rangos de edición: " + ex.Message);
                }
            }



            /*determina los rangos*/
            Excel.Range[] rangos = new Excel.Range[]
            {
                hojaActiva.Range["C48", "D50"], 
                hojaActiva.Range["C56", "D58"], 
                hojaActiva.Range["H48", "I50"],
                hojaActiva.Range["H56", "I58"]  
            };

            Excel.Range rangoSeleccionado = null;

            /*busca el primer rango disponible*/
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

            /*si todos los rangos estan utilizados, usar el ultimo*/
            /* if <= 1,, rango seleccionado =  rango[longitud actual - 1]*/
            if (rangoSeleccionado == null)
                rangoSeleccionado = rangos[rangos.Length - 1];

            float impresionLeft = (float)(double)rangoSeleccionado.Left;
            float impresionTop = (float)(double)rangoSeleccionado.Top;
            float impresionWidth = (float)(double)rangoSeleccionado.Width;
            float impresionHeight = (float)(double)rangoSeleccionado.Height;


            /*definición de la impresión*/
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


            /*bloque de protección*/
            hojaActiva.Protect(Contents: true, DrawingObjects: true, Password:"P5w0soA314641z");
            libroActivo.Protect(Password:"P5w0soA314641z", Structure: true);

            File.Delete(tempPath);


  
                
            
            
        }


        

       
    }
}