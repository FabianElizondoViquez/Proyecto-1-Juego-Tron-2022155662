using System.Drawing;
using System.Windows.Forms;

namespace Proyecto1JuegoTron
{
    public class Nodo
    {
        public Nodo Arriba { get; set; }
        public Nodo Abajo { get; set; }
        public Nodo Izquierda { get; set; }
        public Nodo Derecha { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        public Nodo(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class Grid
    {
        public Nodo[,] Nodos { get; set; }
        public int Filas { get; private set; }
        public int Columnas { get; private set; }

        public Grid(int filas, int columnas)
        {
            Filas = filas;
            Columnas = columnas;
            Nodos = new Nodo[filas, columnas];
            InicializarGrid();
        }

        private void InicializarGrid()
        {
            for (int i = 0; i < Filas; i++)
            {
                for (int j = 0; j < Columnas; j++)
                {
                    Nodos[i, j] = new Nodo(i, j);
                }
            }

            for (int i = 0; i < Filas; i++)
            {
                for (int j = 0; j < Columnas; j++)
                {
                    if (i > 0) Nodos[i, j].Arriba = Nodos[i - 1, j];
                    if (i < Filas - 1) Nodos[i, j].Abajo = Nodos[i + 1, j];
                    if (j > 0) Nodos[i, j].Izquierda = Nodos[i, j - 1];
                    if (j < Columnas - 1) Nodos[i, j].Derecha = Nodos[i, j + 1];
                }
            }
        }
    }

    public partial class Form1 : Form
    {

        private void CrearGrid()
        {
            _tamañoNodo = Math.Min(this.ClientSize.Width / 90, this.ClientSize.Height / 51); // Tamaño del nodo ajustado a la pantalla
            int filas = this.ClientSize.Height / _tamañoNodo;
            int columnas = this.ClientSize.Width / _tamañoNodo;

            _grid = new Grid(filas, columnas);

            if (_posicionMoto != null)
            {
                int filaCentro = Math.Min(filas / 2, _posicionMoto.X);
                int columnaCentro = Math.Min(columnas / 2, _posicionMoto.Y);
                _posicionMoto = _grid.Nodos[filaCentro, columnaCentro];
            }
            else
            {
                int filaCentro = filas / 2;
                int columnaCentro = columnas / 2;
                _posicionMoto = _grid.Nodos[filaCentro, columnaCentro];
            }

            this.Invalidate();
        }

        private void DibujarGrid(Graphics g)
        {
            Color colorLinea = Color.FromArgb(55, 135, 206, 250);
            using (Pen pen = new Pen(colorLinea))
            {
                for (int i = 0; i < _grid.Filas; i++)
                {
                    for (int j = 0; j < _grid.Columnas; j++)
                    {
                        Nodo nodo = _grid.Nodos[i, j];
                        g.DrawRectangle(pen, new Rectangle(nodo.Y * _tamañoNodo, nodo.X * _tamañoNodo, _tamañoNodo, _tamañoNodo));
                    }
                }
            }
        }

    }
}
