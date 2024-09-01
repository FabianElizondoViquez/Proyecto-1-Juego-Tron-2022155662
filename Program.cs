using System;
using System.Windows.Forms;

namespace Proyecto1JuegoTron
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Habilita el uso de estilos visuales para los controles de la aplicación.
            Application.EnableVisualStyles();
            
            // Establece que la aplicación use la representación de texto predeterminada para controles compatibles.
            Application.SetCompatibleTextRenderingDefault(false);

            // Inicia la ejecución de la aplicación creando y mostrando la ventana principal (Form1).
            Application.Run(new Form1());
        }
    }
}
