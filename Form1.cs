using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Proyecto1JuegoTron
{
    /// <summary>
    /// Clase principal del formulario que maneja el juego Tron. Controla la lógica del juego, el movimiento de la moto, la interacción con bots y la interfaz gráfica.
    /// </summary>
    public partial class Form1 : Form
    {
        private Grid _grid;                           // Grid que representa el campo de juego
        private Image _motoAzul;                      // Imagen de la moto controlada por el jugador
        private Image _motoAmarilla;                  // Imagen de los bots
        private Nodo _posicionMoto;                   // Posición actual de la moto del jugador
        private float _anguloRotacion;                // Ángulo de rotación de la moto del jugador
        private int _tamañoNodo;                      // Tamaño de cada nodo en el grid
        private int _anchoMoto;                       // Ancho de la moto en píxeles
        private int _altoMoto;                        // Alto de la moto en píxeles
        private Estela _estelaMoto;                   // Estela dejada por la moto del jugador
        private System.Windows.Forms.Timer _timerMoto;// Timer para controlar el movimiento de la moto
        private Random _random;                       // Generador de números aleatorios
        private int _velocidadMoto;                   // Velocidad de la moto del jugador
        private int _combustibleMoto;                 // Nivel de combustible de la moto del jugador
        private Font _font;                           // Fuente para dibujar texto en la interfaz
        private Brush _brush;                         // Pincel para dibujar texto en la interfaz
        private Panel _infoPanel;                     // Panel que muestra información de la moto (velocidad, combustible)
        private List<Bot> _bots;                      // Lista de bots en el juego
        private System.Windows.Forms.Timer _timerBots;// Timer para controlar el movimiento de los bots

        /// <summary>
        /// Constructor de la clase Form1. Inicializa los componentes, crea el grid, carga las imágenes y configura los timers.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            
            // Configuración del panel de información
            _infoPanel = new Panel
            {
                BackColor = Color.Black,
                BorderStyle = BorderStyle.FixedSingle,
                Size = new Size(200, 50),
                Location = new Point(10, 10) // Ajusta la ubicación según sea necesario
            };
            _infoPanel.Paint += new PaintEventHandler(PanelInfo_Paint);
            this.Controls.Add(_infoPanel);

            // Configuración inicial de la interfaz gráfica
            _font = new Font("Arial", 12);
            _brush = Brushes.White;
            _anguloRotacion = 0;
            this.DoubleBuffered = true; // Evita el parpadeo durante el redibujado
            this.Resize += new EventHandler(Form1_Resize);

            // Crear el grid y cargar las imágenes
            CrearGrid();
            CargarMoto();
            CargarBots();

            // Inicializar el timer para los bots
            InicializarTemporizadorBots();

            // Configurar el evento KeyDown para capturar las teclas de dirección
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);

            // Inicializar la estela de la moto con 3 nodos
            _estelaMoto = new Estela(_posicionMoto);
            _estelaMoto.Siguiente = new Estela(_posicionMoto);
            _estelaMoto.Siguiente.Siguiente = new Estela(_posicionMoto);

            // Configuración inicial de la velocidad y el combustible
            _random = new Random();
            _velocidadMoto = _random.Next(1, 11); // Velocidad aleatoria entre 1 y 10
            _combustibleMoto = 100; // Valor inicial de combustible
            _nodosRecorridos = 0;

            // Crear los bots y asignarles posiciones y velocidades iniciales
            _bots = new List<Bot>();
            for (int i = 0; i < 4; i++)
            {
                Nodo posicionInicial = _grid.Nodos[_random.Next(0, _grid.Filas), _random.Next(0, _grid.Columnas)];
                int velocidadBot = _random.Next(1, 6); // Velocidad aleatoria entre 1 y 5
                Bot bot = new Bot(posicionInicial, _motoAmarilla, 0, _tamañoNodo); // Ángulo inicial de rotación 0
                _bots.Add(bot);
            }

            // Configuración del Timer para el movimiento de la moto
            _timerMoto = new System.Windows.Forms.Timer();
            _timerMoto.Interval = 1100 / _velocidadMoto; // Intervalo de tiempo basado en la velocidad
            _timerMoto.Tick += new EventHandler(Timer_Tick);
            _timerMoto.Start();
        }

        /// <summary>
        /// Evento que se dispara cuando es necesario redibujar el formulario. Dibuja el grid, la estela, la moto del jugador y los bots.
        /// </summary>
        /// <param name="e">Los datos del evento de pintura.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.Clear(Color.Black); // Fondo del grid

            if (_grid != null)
            {
                DibujarGrid(e.Graphics);    // Dibuja el grid
                DibujarEstela(e.Graphics);  // Dibuja la estela de la moto
                DibujarMoto(e.Graphics);    // Dibuja la moto del jugador

                // Dibuja los bots
                foreach (var bot in _bots)
                {
                    bot.Dibujar(e.Graphics, _tamañoNodo);
                }
            }    
            _infoPanel.Invalidate(); // Solicita redibujar el panel de información
        }

        /// <summary>
        /// Evento que se dispara cuando es necesario redibujar el panel de información. Muestra la velocidad y el combustible de la moto.
        /// </summary>
        private void PanelInfo_Paint(object sender, PaintEventArgs e)
        {
            string velocidadTexto = $"Velocidad: {_velocidadMoto}";
            string combustibleTexto = $"Combustible: {_combustibleMoto}";

            e.Graphics.DrawString(velocidadTexto, _font, _brush, new PointF(10, 10));
            e.Graphics.DrawString(combustibleTexto, _font, _brush, new PointF(10, 30));
        }

        /// <summary>
        /// Inicializa el temporizador que controla el movimiento de los bots.
        /// </summary>
        private void InicializarTemporizadorBots()
        {
            _timerBots = new System.Windows.Forms.Timer();
            _timerBots.Interval = 1000; // Intervalo en milisegundos
            _timerBots.Tick += new EventHandler(TimerBots_Tick);
            _timerBots.Start();
        }

        /// <summary>
        /// Evento que se dispara en cada tick del temporizador de los bots. Mueve los bots y redibuja la pantalla.
        /// </summary>
        private void TimerBots_Tick(object sender, EventArgs e)
        {
            foreach (var bot in _bots)
            {
                bot.Mover();
            }

            this.Invalidate(); // Solicita redibujar la pantalla
        }
    }
}
