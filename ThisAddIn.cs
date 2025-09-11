using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;
using System.Windows;

namespace MarkAddIn
{
    public partial class ThisAddIn
    {
        // Reemplaza el bloque en ThisAddIn_Startup y ThisAddIn_Shutdown para corregir CS1061

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {

            // El error CS1061 ocurre porque Application.Current es de WPF, no de Excel Interop.

            // Si necesitas inicializar una aplicación WPF, usa System.Windows.Application.Current.

            // Si solo trabajas con Excel, puedes eliminar esta comprobación.



            // Si tu complemento necesita una instancia de WPF Application:

            if (System.Windows.Application.Current == null)

            {

                new System.Windows.Application();

            }
        }


            // Si no usas WPF, simplemente elimina este bloque.

     
        
        //para despues de descargar el complemento 
        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            // Opcional: cerrar la aplicación WPF si lo deseas
            // Elimina el uso de Application.Current si no usas WPF.
        }

        #region Código generado por VSTO

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
