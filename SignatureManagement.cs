using Mark.Client;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Abstractions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;   

namespace MarkAddIn
{
    public class SignatureManagement
    {
        public SignatureManagement()    /*definiciones para el uso de la clase*/
        {
            Bootstrapper.ConfigureServices();

            var servicios = new ServiceCollection();

            servicios.AddTransient<TokenHander>();
            servicios.AddTransient<TokenManager>();

            TokenHander tokenHander = servicios.BuildServiceProvider().GetRequiredService<TokenHander>();
            TokenManager tokenManager = servicios.BuildServiceProvider().GetRequiredService<TokenManager>();


            /* se guarda el nombre en la variable preDisplayName con el valor que se capturó en .Nombre*/
            var preDisplayName = tokenHander.Nombre;   

        }






        public static void ManageSignature(byte[] firmaBytes, string preDisplayName)
        {
            if (firmaBytes == null || firmaBytes.Length == 0)
                throw new ArgumentException("La firma está vacía o no se generó correctamente.");

            string firmaTemporal = Path.Combine(Path.GetTempPath(), $"firma_{Guid.NewGuid()}.png");
            File.WriteAllBytes(firmaTemporal, firmaBytes);

            var app = Globals.ThisAddIn.Application;
            Excel.Worksheet hojaActiva = (Excel.Worksheet)app.ActiveSheet;


            var appWorkbook = Globals.ThisAddIn.Application;
            Excel.Workbook libroActivo = appWorkbook.ActiveWorkbook;


            hojaActiva.Unprotect(Password: "P5w0soA314641z");


            static void leerRangosdeCeldas()
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

            Excel.Range[] rangosDeLectura  = new Excel.Range[]
            {
                hojaActiva.Range["CD52"],
                hojaActiva.Range["HI52"],
                hojaActiva.Range["CD60"],
                hojaActiva.Range["HI60"]
            };

           
           
            bool firmanteEncontado = false;
            Excel.Range celdaFirmante = null;

            foreach (var rangoDeLectura in rangosDeLectura)
            {
                foreach (Excel.Range celda in rangoDeLectura.Cells)
                {
                    var valorCelda = celda.Value?.ToString();
                    if (!string.IsNullOrEmpty(valorCelda) && valorCelda.Equals(preDisplayName, StringComparison.OrdinalIgnoreCase))
                    {
                        firmanteEncontado = true;
                        celdaFirmante = celda;
                        break;
                    }
                }
                if (firmanteEncontado) break;
            }

            if (firmanteEncontado)
            {
                MessageBox.Show($"{preDisplayName} fue encontrado en los rangos especificados.");

               
                Excel.Range rangoFirma = hojaActiva.Range["E60", "F65"];

                
                hojaActiva.Pictures().Insert(firmaTemporal).Select();
                Excel.Picture imagenFirma = (Excel.Picture)hojaActiva.Pictures(hojaActiva.Pictures().Count);
                imagenFirma.Top = rangoFirma.Top;
                imagenFirma.Left = rangoFirma.Left;
                imagenFirma.Width = rangoFirma.Width;
                imagenFirma.Height = rangoFirma.Height;
            }
            else
            {
                MessageBox.Show($"{preDisplayName}: No tienes permiso para firmar este documento. El complemento se cerrará");
            }



        }
        }
    }





 
 
 
 
 
 
 
 
 
 
