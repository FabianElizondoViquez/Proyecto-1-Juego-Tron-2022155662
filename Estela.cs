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
}
