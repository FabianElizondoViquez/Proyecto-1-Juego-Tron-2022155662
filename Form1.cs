using System;
using System.Drawing;
using System.Windows.Forms;

namespace Proyecto1JuegoTron
{
    public partial class Form1 : Form
    {
        private Grid _grid;
        private Image _motoAzul;
        private Nodo _posicionMoto;
        private float _anguloRotacion;
        private int _tamañoNodo;
        private int _anchoMoto;
        private int _altoMoto;
        private Estela _estelaMoto;

        public Form1()
        {
            InitializeComponent();
            _anguloRotacion = 0;
            this.DoubleBuffered = true;
            this.Resize += new EventHandler(Form1_Resize);
            CrearGrid();
            CargarMoto();
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);

            // Inicializar la estela
            _estelaMoto = new Estela(_posicionMoto);
            _estelaMoto.Siguiente = new Estela(_posicionMoto);
            _estelaMoto.Siguiente.Siguiente = new Estela(_posicionMoto);
        }

        private void CrearGrid()
        {
            _tamañoNodo = Math.Min(this.ClientSize.Width / 90, this.ClientSize.Height / 51); // Tamaño del nodo ajustado a la pantalla
            int filas = this.ClientSize.Height / _tamañoNodo;
            int columnas = this.ClientSize.Width / _tamañoNodo;

            _grid = new Grid(filas, columnas);

            // Si la moto está en una posición válida, mantenla en el nuevo grid
            if (_posicionMoto != null)
            {
                int filaCentro = Math.Min(filas / 2, _posicionMoto.X);
                int columnaCentro = Math.Min(columnas / 2, _posicionMoto.Y);
                _posicionMoto = _grid.Nodos[filaCentro, columnaCentro];
            }
            else
            {
                // Posicionar la moto en el centro si es la primera inicialización
                int filaCentro = filas / 2;
                int columnaCentro = columnas / 2;
                _posicionMoto = _grid.Nodos[filaCentro, columnaCentro];
            }

            this.Invalidate();
        }

        private void CargarMoto()
        {
            try
            {
                _motoAzul = Image.FromFile("motoazul.png");
                _anchoMoto = 35;  // Ancho de la moto
                _altoMoto = 40;   // Alto de la moto
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la imagen de la moto: " + ex.Message);
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            CrearGrid();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.Clear(Color.Black); // Fondo del grid

            if (_grid != null)
            {
                // Crear un color azul claro con 50% de opacidad
                Color colorLinea = Color.FromArgb(55, 135, 206, 250);

                // Crear un Pen con el color semi-transparente
                using (Pen pen = new Pen(colorLinea))
                {
                    for (int i = 0; i < _grid.Filas; i++)
                    {
                        for (int j = 0; j < _grid.Columnas; j++)
                        {
                            Nodo nodo = _grid.Nodos[i, j];
                            e.Graphics.DrawRectangle(pen, new Rectangle(nodo.Y * _tamañoNodo, nodo.X * _tamañoNodo, _tamañoNodo, _tamañoNodo)); // Líneas del grid
                        }
                    }
                }

                // Dibujar estela
                Estela estela = _estelaMoto;
                while (estela != null)
                {
                    if (estela.Nodo != null)
                    {
                        e.Graphics.FillRectangle(Brushes.Turquoise, new Rectangle(
                            estela.Nodo.Y * _tamañoNodo,
                            estela.Nodo.X * _tamañoNodo,
                            _tamañoNodo,
                            _tamañoNodo));
                    }
                    estela = estela.Siguiente;
                }

                if (_posicionMoto != null && _motoAzul != null)
                {
                    // Calcular la posición central de la moto
                    float xCentro = _posicionMoto.Y * _tamañoNodo + _tamañoNodo / 2.0f;
                    float yCentro = _posicionMoto.X * _tamañoNodo + _tamañoNodo / 2.0f;

                    // Configurar la rotación
                    e.Graphics.TranslateTransform(xCentro, yCentro);
                    e.Graphics.RotateTransform(_anguloRotacion);
                    e.Graphics.TranslateTransform(-xCentro, -yCentro);

                    // Dibujar la moto
                    e.Graphics.DrawImage(_motoAzul, new Rectangle(
                        (int)(xCentro - _anchoMoto / 2),
                        (int)(yCentro - _altoMoto / 2),
                        _anchoMoto,
                        _altoMoto));
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Nodo nuevoNodo = null;

            switch (e.KeyCode)
            {
                case Keys.Up:
                    nuevoNodo = _posicionMoto.Arriba;
                    _anguloRotacion = 0;
                    break;
                case Keys.Down:
                    nuevoNodo = _posicionMoto.Abajo;
                    _anguloRotacion = 180;
                    break;
                case Keys.Left:
                    nuevoNodo = _posicionMoto.Izquierda;
                    _anguloRotacion = -90;
                    break;
                case Keys.Right:
                    nuevoNodo = _posicionMoto.Derecha;
                    _anguloRotacion = 90;
                    break;
            }

            if (nuevoNodo != null)
            {
                // Actualizar estela
                Estela nuevaEstela = new Estela(_posicionMoto);
                nuevaEstela.Siguiente = _estelaMoto;
                _estelaMoto = nuevaEstela;

                // Mantener solo 3 posiciones en la estela
                Estela tempEstela = _estelaMoto;
                int count = 0;
                while (tempEstela != null && count < 3)
                {
                    tempEstela = tempEstela.Siguiente;
                    count++;
                }
                if (tempEstela != null)
                {
                    tempEstela.Siguiente = null;
                }

                _posicionMoto = nuevoNodo;
                this.Invalidate();
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
        public Estela Estela { get; set; } // Nueva propiedad

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
