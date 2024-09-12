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

            // Recorremos la estela hasta encontrar el último nodo
            while (actual.Siguiente != null)
            {
                actual = actual.Siguiente;
            }

            // Agregamos un nuevo nodo al final de la estela
            actual.Siguiente = new Estela(nuevoNodo);
        }
    }
}