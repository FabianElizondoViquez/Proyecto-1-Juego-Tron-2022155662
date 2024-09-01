namespace Proyecto1JuegoTron
{
    partial class Form1
    {
        /// <summary>
        /// Contenedor para los componentes del formulario.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpia los recursos que está utilizando el formulario.
        /// </summary>
        /// <param name="disposing">Indica si los recursos administrados deben ser eliminados.</param>
        protected override void Dispose(bool disposing)
        {
            // Si disposing es verdadero y los componentes no son nulos, se eliminan los componentes.
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método requerido para admitir el Diseñador de Windows Forms. 
        /// Configura y inicializa los componentes del formulario.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout(); // Suspende el diseño del formulario mientras se configuran sus propiedades.
            // 
            // Form1
            // 
            // Configuración inicial del formulario, incluyendo tamaño, nombre y texto del título.
            this.ClientSize = new System.Drawing.Size(1366, 768); // Tamaño de la ventana en píxeles
            this.Name = "Form1"; // Nombre del formulario
            this.Text = "Tron Game"; // Texto que aparece en la barra de título del formulario
            this.ResumeLayout(false); // Reanuda el diseño del formulario después de la configuración.
        }

        #endregion
    }
}
