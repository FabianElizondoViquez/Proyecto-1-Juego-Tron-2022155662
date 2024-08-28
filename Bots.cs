using System;
using System.Drawing;
using System.Windows.Forms;

namespace Proyecto1JuegoTron
{
    public class Bot
    {
        public Nodo Posicion { get; set; }
        public float AnguloRotacion { get; set; }
        public Image Imagen { get; set; }
        public int Velocidad { get; set; }
        public int Combustible { get; set; }
        private int _tamañoNodo;
        private Random _random;
        private System.Windows.Forms.Timer _movimientoTimer;

        public Bot(Nodo posicion, Image imagen, float anguloRotacion, int tamañoNodo)
        {
            Posicion = posicion;
            Imagen = imagen;
            AnguloRotacion = anguloRotacion;
            _tamañoNodo = tamañoNodo;
            _random = new Random();

            _movimientoTimer = new System.Windows.Forms.Timer();
            _movimientoTimer.Interval = 1000; // Mover cada segundo
            _movimientoTimer.Tick += (s, e) => Mover();
            _movimientoTimer.Start();
        }

        public void Mover()
        {
            if (Combustible > 0)
            {
                // Movimiento aleatorio
                int direccion = _random.Next(0, 4);
                Nodo nuevoNodo = null;

                switch (direccion)
                {
                    case 0: // Arriba
                        nuevoNodo = Posicion.Arriba;
                        AnguloRotacion = 0;
                        break;
                    case 1: // Abajo
                        nuevoNodo = Posicion.Abajo;
                        AnguloRotacion = 180;
                        break;
                    case 2: // Izquierda
                        nuevoNodo = Posicion.Izquierda;
                        AnguloRotacion = -90;
                        break;
                    case 3: // Derecha
                        nuevoNodo = Posicion.Derecha;
                        AnguloRotacion = 90;
                        break;
                }

                if (nuevoNodo != null)
                {
                    Posicion = nuevoNodo;
                    Combustible -= 1; // Consumir combustible en cada movimiento
                }
            }
        }

        public void Dibujar(Graphics g, int tamañoNodo)
        {
            if (Posicion != null && Imagen != null)
            {
                float xCentro = Posicion.Y * tamañoNodo + tamañoNodo / 2.0f;
                float yCentro = Posicion.X * tamañoNodo + tamañoNodo / 2.0f;

                g.TranslateTransform(xCentro, yCentro);
                g.RotateTransform(AnguloRotacion);
                g.TranslateTransform(-xCentro, -yCentro);

                g.DrawImage(Imagen, new Rectangle(
                    (int)(xCentro - Imagen.Width / 2),
                    (int)(yCentro - Imagen.Height / 2),
                    Imagen.Width,
                    Imagen.Height));
            }
        }
    }
}
