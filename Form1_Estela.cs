using System.Drawing;
using System.Windows.Forms;

namespace Proyecto1JuegoTron
{
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
