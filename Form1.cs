using System;
using System.Drawing;
using System.Windows.Forms;

namespace Proyecto1JuegoTron
{
    public partial class Form1 : Form
    {
        private Grid _grid;

        public Form1()
        {
            InitializeComponent();
            CrearGrid();
        }

        private void CrearGrid()
        {
            int filas = 60; // Calculado según el tamaño de la pantalla y tamaño de las celdas
            int columnas = 34;
            _grid = new Grid(filas, columnas);
            this.Invalidate(); // Redibuja el Form para mostrar el grid
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.Clear(Color.Black); // Fondo negro

            for (int i = 0; i < _grid.Filas; i++)
            {
                for (int j = 0; j < _grid.Columnas; j++)
                {
                    Nodo nodo = _grid.Nodos[i, j];
                    e.Graphics.DrawRectangle(Pens.Turquoise, new Rectangle(nodo.X * 30, nodo.Y * 30, 30, 30)); // Líneas azules
                }
            }
        }
    }

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

            // Establecer referencias
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
}
