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

        // Métodos delegados a archivos específicos:
        // CrearGrid se encuentra en Form1_Grid.cs
        // CargarMoto se encuentra en Form1_Moto.cs
        // DibujarGrid se encuentra en Form1_Grid.cs
        // DibujarEstela se encuentra en Form1_Estela.cs
        // DibujarMoto se encuentra en Form1_Moto.cs
        // Form1_Resize se encuentra en Form1_Eventos.cs
        // Form1_KeyDown se encuentra en Form1_Eventos.cs
        // Timer_Tick se encuentra en Form1_Eventos.cs
    }
}
