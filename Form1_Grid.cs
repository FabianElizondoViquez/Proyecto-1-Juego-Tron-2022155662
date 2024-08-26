using System.Drawing;
using System.Windows.Forms;

namespace Proyecto1JuegoTron
{
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
