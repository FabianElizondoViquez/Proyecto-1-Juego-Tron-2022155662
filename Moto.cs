using System.Drawing;
using System.Windows.Forms;

namespace Proyecto1JuegoTron
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Carga la imagen de la moto del jugador (moto azul) desde un archivo y establece sus dimensiones.
        /// </summary>
        private void CargarMoto()
        {
            try
            {
                // Cargar la imagen de la moto azul desde el archivo
                _motoAzul = Image.FromFile("motoazul.png");
                
                // Establecer el ancho y el alto de la moto
                _anchoMoto = 35;  // Ancho de la moto en píxeles
                _altoMoto = 40;   // Alto de la moto en píxeles
            }
            catch (Exception ex)
            {
                // Mostrar un mensaje de error si ocurre un problema al cargar la imagen
                MessageBox.Show("Error al cargar la imagen de la moto: " + ex.Message);
            }
        }

        /// <summary>
        /// Carga la imagen de los bots (moto amarilla) desde un archivo.
        /// </summary>
        private void CargarBots()
        {
            try
            {
                // Cargar la imagen de la moto amarilla desde el archivo
                _motoAmarilla = Image.FromFile("motoamarilla.png");
            }
            catch (Exception ex)
            {
                // Mostrar un mensaje de error si ocurre un problema al cargar la imagen
                MessageBox.Show("Error al cargar la imagen de la moto amarilla: " + ex.Message);
            }
        }

        /// <summary>
        /// Dibuja la moto del jugador en su posición actual dentro del grid, aplicando una rotación en función del ángulo de rotación.
        /// </summary>
        /// <param name="g">Objeto Graphics utilizado para dibujar en el formulario.</param>
        private void DibujarMoto(Graphics g)
        {
            // Verificar que la posición de la moto y la imagen de la moto estén cargadas
            if (_posicionMoto != null && _motoAzul != null)
            {
                // Calcular el centro de la moto en el grid
                float xCentro = _posicionMoto.Y * _tamañoNodo + _tamañoNodo / 2.0f;
                float yCentro = _posicionMoto.X * _tamañoNodo + _tamañoNodo / 2.0f;

                // Aplicar una transformación para rotar la moto alrededor de su centro
                g.TranslateTransform(xCentro, yCentro);     // Mover el punto de origen al centro de la moto
                g.RotateTransform(_anguloRotacion);        // Rotar la moto según el ángulo especificado
                g.TranslateTransform(-xCentro, -yCentro);  // Mover el punto de origen de vuelta

                // Dibujar la imagen de la moto en el grid en la posición calculada
                g.DrawImage(_motoAzul, new Rectangle(
                    (int)(xCentro - _anchoMoto / 2),
                    (int)(yCentro - _altoMoto / 2),
                    _anchoMoto,
                    _altoMoto));
            }
        }
    }
}
