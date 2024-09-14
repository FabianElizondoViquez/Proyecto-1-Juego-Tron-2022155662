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
        private Nodo _posicionMoto;                   
        private float _anguloRotacion;                
        private int _tamañoNodo;                      
        private int _anchoMoto;                       
        private int _altoMoto;                        
        private Estela _estelaMoto;                   
        private System.Windows.Forms.Timer _timerMoto;
        private Random _random;                       
        private int _velocidadMoto;                   
        private int _velocidadBot;
        private int _combustibleMoto;                 
        private Font _font;                           
        private Brush _brush;                          
        private Panel _infoPanel;                     

        public Form1()
        {
            InitializeComponent();
            _infoPanel = new Panel
            {
                BackColor = Color.Black,
                BorderStyle = BorderStyle.FixedSingle,
                Size = new Size(200, 50),
                Location = new Point(10, 10)
            };
            _infoPanel.Paint += new PaintEventHandler(PanelInfo_Paint);
            this.Controls.Add(_infoPanel);
            _font = new Font("Arial", 12);
            _brush = Brushes.White;
            _anguloRotacion = 0;
            _longitudEstela = 3;
            this.DoubleBuffered = true;
            this.Resize += new EventHandler(Form1_Resize);
            CrearGrid();
            InicializarBots();
            CargarMoto();
            
            InicializarItems();
            CrearItemAleatorio();
            ConfigurarTimerItems();

            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            _estelaMoto = new Estela(_posicionMoto);
            _estelaMoto.Siguiente = new Estela(_posicionMoto);
            _estelaMoto.Siguiente.Siguiente = new Estela(_posicionMoto);

            _random = new Random();
            _velocidadMoto = _random.Next(1, 11);
            _combustibleMoto = 100;
            _nodosRecorridos = 0;

            _timerMoto = new System.Windows.Forms.Timer();
            _timerMoto.Interval = 500 / _velocidadMoto;
            _timerMoto.Tick += new EventHandler(Timer_Tick);
            _timerMoto.Start();

            _timerNuevoBot.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.Clear(Color.Black);

            if (_grid != null)
            {
                DibujarGrid(e.Graphics);
                DibujarEstela(e.Graphics);
                DibujarMoto(e.Graphics);
                DibujarBots(e.Graphics);
            }    
            _infoPanel.Invalidate();
        }

        private void DibujarEstela(Graphics g)
        {
            Estela actual = _estelaMoto;
            int contador = 0;
            while (actual != null && contador < _longitudEstela)
            {
                Nodo nodo = actual.Nodo;
                g.FillRectangle(Brushes.Turquoise, nodo.Y * _tamañoNodo, nodo.X * _tamañoNodo, _tamañoNodo, _tamañoNodo);
                actual = actual.Siguiente;
                contador++;
            }
            Console.WriteLine($"Nodos dibujados en la estela: {contador}");
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