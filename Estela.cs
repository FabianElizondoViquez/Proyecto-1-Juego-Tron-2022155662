using System.Drawing;
using System.Windows.Forms;

namespace Proyecto1JuegoTron
{
    public class Estela
    {
        public Nodo Nodo { get; set; }
        public Estela Siguiente { get; set; }

        public Estela(Nodo nodo)
        {
            Nodo = nodo;
            Siguiente = null;
        }
    }

    public partial class Form1 : Form
    {

        private void DibujarEstela(Graphics g)
        {
            Estela estela = _estelaMoto;
            while (estela != null)
            {
                if (estela.Nodo != null)
                {
                    g.FillRectangle(Brushes.Turquoise, new Rectangle(
                        estela.Nodo.Y * _tamañoNodo,
                        estela.Nodo.X * _tamañoNodo,
                        _tamañoNodo,
                        _tamañoNodo));
                }
                estela = estela.Siguiente;
            }
        }
    }
}
