using System.Drawing;
using System.Windows.Forms;

namespace Proyecto1JuegoTron
{
    /// <summary>
    /// Representa un nodo individual en el grid. Cada nodo tiene referencias a los nodos vecinos (arriba, abajo, izquierda, derecha).
    /// </summary>
    public class Nodo
    {
        /// <summary>
        /// Nodo vecino en la dirección arriba.
        /// </summary>
        public Nodo Arriba { get; set; }

        /// <summary>
        /// Nodo vecino en la dirección abajo.
        /// </summary>
        public Nodo Abajo { get; set; }

        /// <summary>
        /// Nodo vecino en la dirección izquierda.
        /// </summary>
        public Nodo Izquierda { get; set; }

        /// <summary>
        /// Nodo vecino en la dirección derecha.
        /// </summary>
        public Nodo Derecha { get; set; }

        /// <summary>
        /// Coordenada X del nodo en el grid.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Coordenada Y del nodo en el grid.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Constructor de la clase Nodo que inicializa las coordenadas X e Y.
        /// </summary>
        /// <param name="x">Coordenada X del nodo.</param>
        /// <param name="y">Coordenada Y del nodo.</param>
        public Nodo(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    /// <summary>
    /// Clase que representa un grid de nodos. Cada celda del grid es un nodo que tiene referencias a sus nodos vecinos.
    /// </summary>
    public class Grid
    {
        /// <summary>
        /// Matriz de nodos que forman el grid.
        /// </summary>
        public Nodo[,] Nodos { get; set; }

        /// <summary>
        /// Número de filas en el grid.
        /// </summary>
        public int Filas { get; private set; }

        /// <summary>
        /// Número de columnas en el grid.
        /// </summary>
        public int Columnas { get; private set; }

        /// <summary>
        /// Constructor de la clase Grid que inicializa el grid con el número especificado de filas y columnas.
        /// </summary>
        /// <param name="filas">Número de filas del grid.</param>
        /// <param name="columnas">Número de columnas del grid.</param>
        public Grid(int filas, int columnas)
        {
            Filas = filas;
            Columnas = columnas;
            Nodos = new Nodo[filas, columnas];
            InicializarGrid();
        }

        /// <summary>
        /// Inicializa el grid creando los nodos y estableciendo las referencias a los nodos vecinos.
        /// </summary>
        private void InicializarGrid()
        {
            // Crear los nodos del grid
            for (int i = 0; i < Filas; i++)
            {
                for (int j = 0; j < Columnas; j++)
                {
                    Nodos[i, j] = new Nodo(i, j);
                }
            }

            // Establecer las referencias a los nodos vecinos
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
        /// <summary>
        /// Crea y configura el grid de nodos, ajustando el tamaño del nodo a las dimensiones actuales de la ventana.
        /// </summary>
        private void CrearGrid()
        {
            // Calcular el tamaño de cada nodo en función del tamaño de la ventana
            _tamañoNodo = Math.Min(this.ClientSize.Width / 90, this.ClientSize.Height / 51);
            int filas = this.ClientSize.Height / _tamañoNodo;
            int columnas = this.ClientSize.Width / _tamañoNodo;

            // Crear el grid con las filas y columnas calculadas
            _grid = new Grid(filas, columnas);

            // Ajustar la posición de la moto al centro del grid
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

            // Forzar la redibujación de la pantalla
            this.Invalidate();
        }

        /// <summary>
        /// Dibuja el grid en el formulario utilizando un color de línea específico.
        /// </summary>
        /// <param name="g">Objeto Graphics utilizado para dibujar en el formulario.</param>
        private void DibujarGrid(Graphics g)
        {
            // Definir el color de las líneas del grid
            Color colorLinea = Color.FromArgb(55, 135, 206, 250);
            using (Pen pen = new Pen(colorLinea))
            {
                // Dibujar cada nodo como un rectángulo en el grid
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
