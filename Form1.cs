using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Proyecto1JuegoTron
{
    public partial class Form1 : Form
    {
        private Grid _grid;                           // Grid que representa el campo de juego
        private Image _motoAzul;                      // Imagen de la moto controlada por el jugador
        private Nodo _posicionMoto;                   // Posición actual de la moto del jugador
        private float _anguloRotacion;                // Ángulo de rotación de la moto del jugador
        private int _tamañoNodo;                      // Tamaño de cada nodo en el grid
        private int _anchoMoto;                       // Ancho de la moto en píxeles
        private int _altoMoto;                        // Alto de la moto en píxeles                  // Alto del Bot en píxeles
        private Estela _estelaMoto;                   // Estela dejada por la moto del jugador
        private System.Windows.Forms.Timer _timerMoto;// Timer para controlar el movimiento de la moto
        private Random _random;                       // Generador de números aleatorios
        private int _velocidadMoto;                   // Velocidad de la moto del jugador
        private int _velocidadBot;
        private int _combustibleMoto;                 // Nivel de combustible de la moto del jugador
        private Font _font;                           // Fuente para dibujar texto en la interfaz
        private Brush _brush;                         // Pincel para dibujar texto en la interfaz
        private Panel _infoPanel;                     // Panel que muestra información de la moto (velocidad, combustible)

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
            _longitudEstela = 3;
            this.DoubleBuffered = true; // Evita el parpadeo durante el redibujado
            this.Resize += new EventHandler(Form1_Resize);
            CrearGrid();
            InicializarBots();  // Inicializar bots
            CargarMoto();
            
            InicializarItems();
            CrearItemAleatorio();
            ConfigurarTimerItems();

            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            _estelaMoto = new Estela(_posicionMoto);
            _estelaMoto.Siguiente = new Estela(_posicionMoto);
            _estelaMoto.Siguiente.Siguiente = new Estela(_posicionMoto);

            _random = new Random();
            _velocidadMoto = _random.Next(1, 11); // Velocidad aleatoria entre 1 y 10
            _combustibleMoto = 100; // Valor inicial de combustible
            _nodosRecorridos = 0;

            _timerMoto = new System.Windows.Forms.Timer();
            _timerMoto.Interval = 500 / _velocidadMoto; // Intervalo de tiempo basado en la velocidad
            _timerMoto.Tick += new EventHandler(Timer_Tick);
            _timerMoto.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.Clear(Color.Black); // Fondo del grid

            if (_grid != null)
            {
                DibujarGrid(e.Graphics);    // Dibuja el grid
                DibujarEstela(e.Graphics);  // Dibuja la estela de la moto
                DibujarMoto(e.Graphics);    // Dibuja la moto del jugador
                DibujarBots(e.Graphics);    // Dibuja los bots
            }    
            _infoPanel.Invalidate(); // Solicita redibujar el panel de información
        }

        private void DibujarEstela(Graphics g)
        {
            Estela actual = _estelaMoto;
            int contador = 0;
            while (actual != null && contador < _longitudEstela)
            {
                Nodo nodo = actual.Nodo;
                g.FillRectangle(Brushes.White, nodo.Y * _tamañoNodo, nodo.X * _tamañoNodo, _tamañoNodo, _tamañoNodo);
                actual = actual.Siguiente;
                contador++;
            }
            Console.WriteLine($"Nodos dibujados en la estela: {contador}"); // Mensaje de depuración
        }

        private void PanelInfo_Paint(object sender, PaintEventArgs e)
        {
            string velocidadTexto = $"Velocidad: {_velocidadMoto}";
            string combustibleTexto = $"Combustible: {_combustibleMoto}";

            e.Graphics.DrawString(velocidadTexto, _font, _brush, new PointF(10, 10));
            e.Graphics.DrawString(combustibleTexto, _font, _brush, new PointF(10, 30));
        }
    }
}