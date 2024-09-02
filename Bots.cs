using System;
using System.Drawing;
using System.Windows.Forms;

namespace Proyecto1JuegoTron
{
    /// <summary>
    /// Representa un bot en el juego Tron, que se mueve automáticamente en un grid.
    /// </summary>
    /// 
    public class Bot
    {
        /// <summary>
        /// Nodo actual en el grid donde se encuentra el bot.
        /// </summary>
        public Nodo Posicion { get; set; }

        /// <summary>
        /// Ángulo de rotación de la imagen del bot, en grados.
        /// </summary>
        public float AnguloRotacion { get; set; }

        /// <summary>
        /// Imagen que representa al bot en el grid.
        /// </summary>
        public Image Imagen { get; set; }

        /// <summary>
        /// Velocidad a la que se mueve el bot.
        /// </summary>
        public int Velocidad { get; set; }

        /// <summary>
        /// Cantidad de combustible disponible para el bot.
        /// </summary>
        public int Combustible { get; set; }

        /// <summary>
        /// Tamaño de cada nodo en el grid.
        /// </summary>
        private int _tamañoNodo;

        /// <summary>
        /// Instancia de la clase Random para generar valores aleatorios.
        /// </summary>
        private Random _random;

        /// <summary>
        /// Timer que controla el movimiento automático del bot.
        /// </summary>
        private System.Windows.Forms.Timer _movimientoTimer;

        /// <summary>
        /// Constructor que inicializa un nuevo bot en una posición determinada del grid.
        /// </summary>
        /// <param name="posicion">Nodo inicial donde se colocará el bot.</param>
        /// <param name="imagen">Imagen que representará al bot.</param>
        /// <param name="anguloRotacion">Ángulo de rotación inicial del bot.</param>
        /// <param name="tamañoNodo">Tamaño de cada nodo en el grid.</param>
        public Bot(Nodo posicion, Image imagen, float anguloRotacion, int tamañoNodo)
        {
            Posicion = posicion;
            Imagen = imagen;
            AnguloRotacion = anguloRotacion;
            _tamañoNodo = tamañoNodo;
            _random = new Random();

            // Configuración del timer para mover el bot cada segundo
            _movimientoTimer = new System.Windows.Forms.Timer();
            _movimientoTimer.Interval = 1000; // Mover cada segundo
            _movimientoTimer.Tick += (s, e) => Mover();
            _movimientoTimer.Start();
        }

        /// <summary>
        /// Mueve el bot a una nueva posición en el grid, basada en una dirección aleatoria.
        /// Consume combustible en cada movimiento.
        /// </summary>
        public void Mover()
        {
            if (Combustible > 0)
            {
                // Generar una dirección aleatoria para mover el bot
                int direccionAleatoria = new Random().Next(0, 4); // 0 = Arriba, 1 = Abajo, 2 = Izquierda, 3 = Derecha
                Nodo nuevoNodo = null;

                switch (direccionAleatoria)
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

                // Si el nuevo nodo no es nulo, mover el bot a esa posición y reducir el combustible
                if (nuevoNodo != null)
                {
                    Posicion = nuevoNodo;
                    Combustible -= 1; // Consumir combustible en cada movimiento
                }
            }
        }

        /// <summary>
        /// Dibuja el bot en el grid, aplicando la rotación y posición correspondientes.
        /// </summary>
        /// <param name="g">El objeto Graphics usado para dibujar.</param>
        /// <param name="tamañoNodo">Tamaño de cada nodo en el grid.</param>
        public void Dibujar(Graphics g, int tamañoNodo)
        {
            if (Posicion != null && Imagen != null)
            {
                // Calcular el centro de la posición actual del bot
                float xCentro = Posicion.Y * tamañoNodo + tamañoNodo / 2.0f;
                float yCentro = Posicion.X * tamañoNodo + tamañoNodo / 2.0f;

                // Aplicar transformaciones para rotar la imagen del bot
                g.TranslateTransform(xCentro, yCentro);
                g.RotateTransform(AnguloRotacion);
                g.TranslateTransform(-xCentro, -yCentro);

                // Dibujar la imagen del bot en la posición calculada
                g.DrawImage(Imagen, new Rectangle(
                    (int)(xCentro - Imagen.Width / 2),
                    (int)(yCentro - Imagen.Height / 2),
                    Imagen.Width,
                    Imagen.Height));
            }
        }
    }
}
