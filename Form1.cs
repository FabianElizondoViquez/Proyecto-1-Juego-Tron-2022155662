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
        private System.Windows.Forms.Timer _timer;

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

            // Configurar el Timer para mover la moto automáticamente
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 100; // Intervalo en milisegundos
            _timer.Tick += new EventHandler(Timer_Tick);
            _timer.Start();
        }

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
                DibujarGrid(e.Graphics);
                DibujarEstela(e.Graphics);
                DibujarMoto(e.Graphics);
            }
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

        private void DibujarMoto(Graphics g)
        {
            if (_posicionMoto != null && _motoAzul != null)
            {
                float xCentro = _posicionMoto.Y * _tamañoNodo + _tamañoNodo / 2.0f;
                float yCentro = _posicionMoto.X * _tamañoNodo + _tamañoNodo / 2.0f;

                g.TranslateTransform(xCentro, yCentro);
                g.RotateTransform(_anguloRotacion);
                g.TranslateTransform(-xCentro, -yCentro);

                g.DrawImage(_motoAzul, new Rectangle(
                    (int)(xCentro - _anchoMoto / 2),
                    (int)(yCentro - _altoMoto / 2),
                    _anchoMoto,
                    _altoMoto));
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    _anguloRotacion = 0;
                    break;
                case Keys.Down:
                    _anguloRotacion = 180;
                    break;
                case Keys.Left:
                    _anguloRotacion = -90;
                    break;
                case Keys.Right:
                    _anguloRotacion = 90;
                    break;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Nodo nuevoNodo = null;

            switch (_anguloRotacion)
            {
                case 0:
                    nuevoNodo = _posicionMoto.Arriba;
                    break;
                case 180:
                    nuevoNodo = _posicionMoto.Abajo;
                    break;
                case -90:
                    nuevoNodo = _posicionMoto.Izquierda;
                    break;
                case 90:
                    nuevoNodo = _posicionMoto.Derecha;
                    break;
            }

            if (nuevoNodo != null)
            {
                Estela nuevaEstela = new Estela(_posicionMoto);
                nuevaEstela.Siguiente = _estelaMoto;
                _estelaMoto = nuevaEstela;

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
}
