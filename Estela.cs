using System.Drawing;
using System.Windows.Forms;

namespace Proyecto1JuegoTron
{
    /// <summary>
    /// Representa un nodo de la estela que deja una moto en el grid.
    /// </summary>
    public class Estela
    {
        /// <summary>
        /// Nodo actual en el grid asociado con esta parte de la estela.
        /// </summary>
        public Nodo Nodo { get; set; }

        /// <summary>
        /// Referencia a la siguiente parte de la estela.
        /// </summary>
        public Estela Siguiente { get; set; }

        /// <summary>
        /// Constructor que inicializa una nueva parte de la estela en un nodo específico.
        /// </summary>
        /// <param name="nodo">Nodo del grid donde se encuentra esta parte de la estela.</param>
        public Estela(Nodo nodo)
        {
            Nodo = nodo;
            Siguiente = null;
        }
    }

    public partial class Form1 : Form
    {
        /// <summary>
        /// Dibuja la estela de la moto en el grid.
        /// </summary>
        /// <param name="g">Objeto Graphics usado para realizar el dibujo.</param>
        private void DibujarEstela(Graphics g)
        {
            // Comienza desde la primera parte de la estela
            Estela estela = _estelaMoto;
            while (estela != null)
            {
                if (estela.Nodo != null)
                {
                    // Dibuja un rectángulo que representa la estela en el nodo actual
                    g.FillRectangle(Brushes.Turquoise, new Rectangle(
                        estela.Nodo.Y * _tamañoNodo,
                        estela.Nodo.X * _tamañoNodo,
                        _tamañoNodo,
                        _tamañoNodo));
                }
                // Avanza a la siguiente parte de la estela
                estela = estela.Siguiente;
            }
        }
    }
}
