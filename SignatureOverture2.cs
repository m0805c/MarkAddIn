using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace MarkAddIn
{
    public class SignatureOverture2
    {
        public static void InserccionPorClic (byte[] firmaBytes)
        {
            if (firmaBytes == null || firmaBytes.Length == 0)

                throw new ArgumentNullException("la firma esta vacia");

            string tempPath = Path.Combine(Path.GetTempPath(), $"firma_{Guid.NewGuid()}.png");
            File.WriteAllBytes(tempPath, firmaBytes);




            var app = Globals.ThisAddIn.Application;
            Excel.Worksheet hojaActiva = (Excel.Worksheet)app.ActiveSheet;

            var appWorkbook = Globals.ThisAddIn.Application;
            Excel.Workbook libroActivo = (Excel.Workbook)app.ActiveWorkbook;







        }
    }
}
