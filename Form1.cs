using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Proyecto1JuegoTron
{
    public partial class Form1 : Form
    {
        private Grid _grid;
        private Image _motoAzul;
        private Image _motoAmarilla;
        private Nodo _posicionMoto;
        private float _anguloRotacion;
        private int _tamañoNodo;
        private int _anchoMoto;
        private int _altoMoto;
        private Estela _estelaMoto;
        private System.Windows.Forms.Timer _timer;
        private Random _random;
        private int _velocidadMoto;
        private int _combustibleMoto;
        private Font _font;
        private Brush _brush;
        private Panel _infoPanel;
        private List<Bot> _bots;
        private System.Windows.Forms.Timer _timerBots;

        public Form1()
        {
            InitializeComponent();
            _infoPanel = new Panel
            {
                BackColor = Color.Black,
                BorderStyle = BorderStyle.FixedSingle,
                Size = new Size(200, 50),
                Location = new Point(10, 10) // Ajusta la ubicación según sea necesario
            };
            _infoPanel.Paint += new PaintEventHandler(PanelInfo_Paint);
            this.Controls.Add(_infoPanel);
            _font = new Font("Arial", 12);
            _brush = Brushes.White;
            _anguloRotacion = 0;
            this.DoubleBuffered = true;
            this.Resize += new EventHandler(Form1_Resize);
            CrearGrid();
            CargarMoto();
            CargarBots(); // Cargar la imagen de los bots
            InicializarTemporizadorBots();
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);

            // Inicializar la estela
            _estelaMoto = new Estela(_posicionMoto);
            _estelaMoto.Siguiente = new Estela(_posicionMoto);
            _estelaMoto.Siguiente.Siguiente = new Estela(_posicionMoto);

            // Configurar la velocidad aleatoria de la moto
            _random = new Random();
            _velocidadMoto = _random.Next(1, 11); // Valor aleatorio entre 1 y 10
            // Configurar el combustible de la moto
            _combustibleMoto = 100; // Valor inicial de combustible
            _nodosRecorridos = 0;

            _bots = new List<Bot>();
            for (int i = 0; i < 4; i++)
            {
                Nodo posicionInicial = _grid.Nodos[_random.Next(0, _grid.Filas), _random.Next(0, _grid.Columnas)];
                int velocidadBot = _random.Next(1, 6); // Velocidad aleatoria entre 1 y 5
                Bot bot = new Bot(posicionInicial, _motoAmarilla, 0, _tamañoNodo); // El ángulo de rotación inicial se establece en 0
                _bots.Add(bot);
            }
            // Configurar el Timer para mover la moto automáticamente
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 1100 / _velocidadMoto; // Inversamente proporcional a la velocidad
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
                foreach (var bot in _bots)
                {
                    bot.Dibujar(e.Graphics, _tamañoNodo);
                }
            }    
            _infoPanel.Invalidate();
        }

        private void PanelInfo_Paint(object sender, PaintEventArgs e)
        {
            string velocidadTexto = $"Velocidad: {_velocidadMoto}";
            string combustibleTexto = $"Combustible: {_combustibleMoto}";

            e.Graphics.DrawString(velocidadTexto, _font, _brush, new PointF(10, 10));
            e.Graphics.DrawString(combustibleTexto, _font, _brush, new PointF(10, 30));
        }

        private void InicializarTemporizadorBots()
        {
            _timerBots = new System.Windows.Forms.Timer();
            _timerBots.Interval = 1000; // Intervalo en milisegundos
            _timerBots.Tick += new EventHandler(TimerBots_Tick);
            _timerBots.Start();
        }
        private void TimerBots_Tick(object sender, EventArgs e)
        {
            foreach (var bot in _bots)
            {
                bot.Mover();
            }

            this.Invalidate(); // Redibujar la pantalla
        }
    }
}
