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
    // Clase para los argumentos del evento de protección
    public class ProtectionEventArgs : EventArgs
    {
        public Excel.Worksheet Worksheet { get; set; }
        public bool DrawingObjectsProtected { get; set; }
        public string Password { get; set; }
        public DateTime Timestamp { get; set; }
    }

    // Clase para los argumentos del evento de inserción de firma
    public class SignatureInsertedEventArgs : EventArgs
    {
        public string UserName { get; set; }
        public Excel.Range TargetCell { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class SignatureOverture
    {
        public static string NombreAutenticado;
        private static string tempPathName;
        private static string tempPath;
        private const string PASSWORD = "P5w0soA314641z";

        public static Excel.Application app = Globals.ThisAddIn.Application;

        #region Eventos
        /// <summary>
        /// Se dispara cuando cambia el estado de protección de DrawingObjects
        /// </summary>
        public static event EventHandler<ProtectionEventArgs> ProtectionChanged;

        /// <summary>
        /// Se dispara cuando la firma se inserta exitosamente
        /// </summary>
        public static event EventHandler<SignatureInsertedEventArgs> SignatureInserted;

        /// <summary>
        /// Se dispara cuando la inserción se cancela o falla
        /// </summary>
        public static event EventHandler<EventArgs> InsertionCancelled;

        /// <summary>
        /// Se dispara antes de desproteger la hoja (útil para validaciones)
        /// </summary>
        public static event EventHandler<ProtectionEventArgs> BeforeUnprotect;

        /// <summary>
        /// Se dispara después de proteger la hoja
        /// </summary>
        public static event EventHandler<ProtectionEventArgs> AfterProtect;
        #endregion

        #region Métodos privados para disparar eventos
        private static void OnProtectionChanged(Excel.Worksheet worksheet, bool drawingObjectsProtected)
        {
            ProtectionChanged?.Invoke(null, new ProtectionEventArgs
            {
                Worksheet = worksheet,
                DrawingObjectsProtected = drawingObjectsProtected,
                Password = PASSWORD,
                Timestamp = DateTime.Now
            });
        }

        private static void OnBeforeUnprotect(Excel.Worksheet worksheet, bool currentProtectionState)
        {
            BeforeUnprotect?.Invoke(null, new ProtectionEventArgs
            {
                Worksheet = worksheet,
                DrawingObjectsProtected = currentProtectionState,
                Password = PASSWORD,
                Timestamp = DateTime.Now
            });
        }

        private static void OnAfterProtect(Excel.Worksheet worksheet, bool protectionState)
        {
            AfterProtect?.Invoke(null, new ProtectionEventArgs
            {
                Worksheet = worksheet,
                DrawingObjectsProtected = protectionState,
                Password = PASSWORD,
                Timestamp = DateTime.Now
            });
        }

        private static void OnSignatureInserted(Excel.Range targetCell)
        {
            SignatureInserted?.Invoke(null, new SignatureInsertedEventArgs
            {
                UserName = NombreAutenticado,
                TargetCell = targetCell,
                Timestamp = DateTime.Now
            });
        }

        private static void OnInsertionCancelled()
        {
            InsertionCancelled?.Invoke(null, EventArgs.Empty);
        }
        #endregion

        private static void LockExistingShapes(Excel.Worksheet hoja)
        {
            foreach (Excel.Shape shape in hoja.Shapes)
            {
                try
                {
                    shape.Locked = true;
                    shape.LockAspectRatio = MsoTriState.msoFalse;
                    shape.Placement = Excel.XlPlacement.xlFreeFloating;
                }
                catch { }
            }
        }

        /// <summary>
        /// Método centralizado para aplicar protección con eventos
        /// </summary>
        private static void ApplyProtection(Excel.Worksheet hoja, bool protectDrawingObjects)
        {
            try
            {
                // Disparar evento antes de desproteger
                OnBeforeUnprotect(hoja, protectDrawingObjects);

                // Desproteger la hoja
                hoja.Unprotect(PASSWORD);
            }
            catch { }

            // Aplicar nueva protección
            hoja.Protect(
                PASSWORD,
                DrawingObjects: protectDrawingObjects,
                Contents: true,
                Scenarios: true,
                UserInterfaceOnly: true
            );

            // Disparar eventos después de proteger
            OnAfterProtect(hoja, protectDrawingObjects);
            OnProtectionChanged(hoja, protectDrawingObjects);
        }

        /// <summary>
        /// Activa el modo de inserción de firma por doble clic
        /// </summary>
        public static void ActivarInserccionPorClic(byte[] firmaBytes)
        {
            if (string.IsNullOrWhiteSpace(NombreAutenticado))
                throw new ArgumentException("El nombre está vacío.");

            if (firmaBytes == null || firmaBytes.Length == 0)
                throw new ArgumentException("Firma vacía.");

            // Crear archivo temporal para la firma
            tempPath = Path.Combine(Path.GetTempPath(), $"firma_{Guid.NewGuid()}.png");
            File.WriteAllBytes(tempPath, firmaBytes);

            // Crear archivo temporal para el nombre
            tempPathName = Path.Combine(Path.GetTempPath(), $"Nombre_{Guid.NewGuid()}.png");
            RenderTextToPng(tempPathName);

            var hojaActiva = (Excel.Worksheet)app.ActiveSheet;

            // Bloquear todas las formas existentes
            LockExistingShapes(hojaActiva);

            // Proteger con DrawingObjects = true inicialmente
            ApplyProtection(hojaActiva, protectDrawingObjects: true);

            // Suscribir al evento de doble clic
            app.SheetBeforeDoubleClick += OnSheetBeforeDoubleClick;

            MessageBox.Show("Doble clic donde deseas insertar la firma.");
        }

        /// <summary>
        /// Renderiza el nombre del usuario como imagen PNG
        /// </summary>
        private static void RenderTextToPng(string outputPath)
        {
            int width = 350, height = 80;

            using Bitmap bitmap = new(width, height);
            using Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);
            using Font font = new("Segoe UI", 16);
            using Brush brush = new SolidBrush(Color.Black);

            SizeF textSize = graphics.MeasureString(NombreAutenticado, font);
            float x = (width - textSize.Width) / 2;
            float y = (height - textSize.Height) / 2;

            graphics.DrawString(NombreAutenticado, font, brush, new PointF(x, y));
            bitmap.Save(outputPath, ImageFormat.Png);
        }

        /// <summary>
        /// Manejador del evento de doble clic en la hoja
        /// </summary>
        private static void OnSheetBeforeDoubleClick(object Sh, Excel.Range Target, ref bool Cancel)
        {
            Cancel = true;
            app.SheetBeforeDoubleClick -= OnSheetBeforeDoubleClick;

            try
            {
                Excel.Worksheet hoja = (Excel.Worksheet)Sh;

                // PASO 1: Desproteger DrawingObjects temporalmente para permitir inserción
                ApplyProtection(hoja, protectDrawingObjects: false);

                // PASO 2: Insertar firma y nombre
                InsertarFirmaEnCelda(hoja, Target);
                InsertarNombreAutenticado(hoja, Target);

                // PASO 3: Bloquear todas las formas (incluidas las recién insertadas)
                LockExistingShapes(hoja);

                // PASO 4: Volver a proteger DrawingObjects
                ApplyProtection(hoja, protectDrawingObjects: true);

                // PASO 5: Disparar evento de firma insertada exitosamente
                OnSignatureInserted(Target);

                MessageBox.Show("Firma insertada correctamente.");
            }
            catch (Exception ex)
            {
                // Disparar evento de inserción cancelada
                OnInsertionCancelled();
                MessageBox.Show("Error al insertar la firma: " + ex.Message);
            }
            finally
            {
                // Limpiar archivos temporales
                FinalizarInserccion();
            }
        }

        /// <summary>
        /// Inserta la imagen de la firma en la celda especificada
        /// </summary>
        private static void InsertarFirmaEnCelda(Excel.Worksheet hoja, Excel.Range celda)
        {
            var shape = hoja.Shapes.AddPicture(
                tempPath,
                MsoTriState.msoFalse,
                MsoTriState.msoTrue,
                (float)celda.Left,
                (float)celda.Top,
                160f,
                60f
            );

            shape.Locked = true;
            shape.LockAspectRatio = MsoTriState.msoTrue;
            shape.Placement = Excel.XlPlacement.xlMoveAndSize;
        }

        /// <summary>
        /// Inserta el nombre del usuario autenticado debajo de la firma
        /// </summary>
        private static void InsertarNombreAutenticado(Excel.Worksheet hoja, Excel.Range celda)
        {
            var shape = hoja.Shapes.AddPicture(
                tempPathName,
                MsoTriState.msoFalse,
                MsoTriState.msoTrue,
                (float)celda.Left,
                (float)celda.Top + 75f,
                200f,
                40f
            );

            shape.Locked = true;
            shape.LockAspectRatio = MsoTriState.msoTrue;
            shape.Placement = Excel.XlPlacement.xlMoveAndSize;
        }

        /// <summary>
        /// Elimina los archivos temporales creados
        /// </summary>
        private static void FinalizarInserccion()
        {
            try
            {
                if (File.Exists(tempPath)) File.Delete(tempPath);
                if (File.Exists(tempPathName)) File.Delete(tempPathName);
            }
            catch { }
        }
    }
}