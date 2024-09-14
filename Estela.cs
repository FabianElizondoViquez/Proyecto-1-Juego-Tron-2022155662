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

        public void AgregarNodo(Nodo nuevoNodo)
        {
            Estela actual = this;

            while (actual.Siguiente != null)
            {
                actual = actual.Siguiente;
            }

            actual.Siguiente = new Estela(nuevoNodo);
        }
    }
}