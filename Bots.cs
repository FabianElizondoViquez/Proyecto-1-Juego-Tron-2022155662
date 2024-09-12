using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Proyecto1JuegoTron
{
    public partial class Form1 : Form
    {
        private Image _motoAmarilla;                    // Imagen de la moto controlada por los bots
        private Nodo[] _posicionesBots;                 // Posiciones actuales de los bots
        private int[] _direccionesBots;                 // Direcciones de movimiento de los bots (0 = Arriba, 1 = Derecha, 2 = Abajo, 3 = Izquierda)
        private int[] _pasosRestantesBots;              // Pasos restantes antes de cambiar de dirección
        private float[] _angulosRotacionBots;           // Ángulos de rotación de cada bot (0, 90, 180, 270 grados)
        private Random _randomBot;                      // Generador de números aleatorios para los bots
        private System.Windows.Forms.Timer _timerBots;  // Timer para controlar el movimiento de los bots
        private List<Nodo>[] _estelasBots;              // Estelas de cada bot (cada lista contiene los nodos de la estela)

        private void InicializarBots()
        {
            _posicionesBots = new Nodo[4];              // Cuatro bots
            _direccionesBots = new int[4];              // Direcciones de movimiento de los bots
            _pasosRestantesBots = new int[4];           // Pasos que darán antes de cambiar de dirección
            _angulosRotacionBots = new float[4];        // Ángulos de rotación de cada bot (0, 90, 180, 270 grados)
            _estelasBots = new List<Nodo>[4];           // Estelas de los bots
            _randomBot = new Random();

            // Cargar la imagen de la moto amarilla
            try
            {
                _motoAmarilla = Image.FromFile("motoamarilla.png");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la imagen de la moto amarilla: " + ex.Message);
            }

            // Colocar a los bots en posiciones iniciales aleatorias
            for (int i = 0; i < _posicionesBots.Length; i++)
            {
                _posicionesBots[i] = _grid.ObtenerNodo(_randomBot.Next(0, _grid.Filas), _randomBot.Next(0, _grid.Columnas));
                _direccionesBots[i] = _randomBot.Next(0, 4); // Direcciones aleatorias: 0 = Arriba, 1 = Derecha, 2 = Abajo, 3 = Izquierda
                _pasosRestantesBots[i] = _randomBot.Next(5, 15); // Un número aleatorio de pasos antes de cambiar de dirección
                _angulosRotacionBots[i] = DireccionARotacion(_direccionesBots[i]); // Rotación inicial del bot
                _estelasBots[i] = new List<Nodo>();  // Inicializamos una lista vacía para la estela de cada bot
            }

            _velocidadBot = _randomBot.Next(1, 11); // Velocidad aleatoria entre 1 y 10
            // Timer para el movimiento de los bots
            _timerBots = new System.Windows.Forms.Timer();
            _timerBots.Interval = 500 / _velocidadBot; // Movimiento cada 200ms
#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
            _timerBots.Tick += new EventHandler(TimerBots_Tick);
#pragma warning restore CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
            _timerBots.Start();
        }

        private void DibujarBots(Graphics g)
        {
            if (_posicionesBots != null && _motoAmarilla != null)
            {
                for (int i = 0; i < _posicionesBots.Length; i++)
                {
                    if (_posicionesBots[i] != null)
                    {
                        // Dibujar la estela del bot
                        if (_estelasBots[i] != null)
                        {
                            foreach (Nodo nodo in _estelasBots[i])
                            {
                                if (nodo != null)
                                {
                                    Brush brochaAmarilla = new SolidBrush(Color.Yellow);
                                    g.FillRectangle(brochaAmarilla, new Rectangle(
                                        nodo.Y * _tamañoNodo, 
                                        nodo.X * _tamañoNodo, 
                                        _tamañoNodo, 
                                        _tamañoNodo));
                                }
                            }
                        }

                        // Dibujar la moto del bot con su rotación correspondiente
                        float xCentro = _posicionesBots[i].Y * _tamañoNodo + _tamañoNodo / 2.0f;
                        float yCentro = _posicionesBots[i].X * _tamañoNodo + _tamañoNodo / 2.0f;
                        g.TranslateTransform(xCentro, yCentro);       // Mover el origen al centro de la moto
                        g.RotateTransform(_angulosRotacionBots[i]);  // Rotar la moto según el ángulo del bot
                        g.TranslateTransform(-xCentro, -yCentro);     // Restaurar origen
                        g.DrawImage(_motoAmarilla, new Rectangle(
                            (int)(xCentro - _anchoMoto / 2),
                            (int)(yCentro - _altoMoto / 2),
                            _anchoMoto,
                            _altoMoto));
                        g.ResetTransform(); // Resetear la transformación gráfica para el siguiente bot
                    }
                }
            }
        }

        private void TimerBots_Tick(object sender, EventArgs e)
        {
            try
            {
                // Crear una lista para las nuevas posiciones de los bots
                Nodo[] nuevasPosiciones = new Nodo[_posicionesBots.Length];
                for (int i = 0; i < _posicionesBots.Length; i++)
                {
                    if (_posicionesBots[i] != null)
                    {
                        nuevasPosiciones[i] = MoverBotEnDireccion(_posicionesBots[i], _direccionesBots[i]);
                    }
                }

                for (int i = 0; i < _posicionesBots.Length; i++)
                {
                    // Agregar la posición actual a la estela
                    if (_estelasBots[i] != null)
                    {
                        _estelasBots[i].Insert(0, _posicionesBots[i]);

                        // Limitar la estela a 3 nodos
                        if (_estelasBots[i].Count > 3)
                        {
                            _estelasBots[i].RemoveAt(_estelasBots[i].Count - 1);
                        }
                    }

                    if (nuevasPosiciones[i] != null && !EsPosicionOcupadaPorOtroBot(nuevasPosiciones[i]))
                    {
                        // Actualizar la posición del bot
                        _posicionesBots[i] = nuevasPosiciones[i];
                        
                        // Cambiar dirección de forma aleatoria
                        if (--_pasosRestantesBots[i] <= 0)
                        {
                            _direccionesBots[i] = _randomBot.Next(0, 4); // Nueva dirección aleatoria
                            _angulosRotacionBots[i] = DireccionARotacion(_direccionesBots[i]); // Actualizar ángulo de rotación
                            _pasosRestantesBots[i] = _randomBot.Next(5, 15); // Nuevos pasos restantes antes del próximo cambio
                        }
                    }
                    else
                    {
                        // Si la nueva posición está ocupada, el bot cambia de dirección aleatoriamente
                        _direccionesBots[i] = _randomBot.Next(0, 4);
                        _angulosRotacionBots[i] = DireccionARotacion(_direccionesBots[i]);
                    }
                }

                VerificarColisiones(); // Añade esta línea aquí
                VerificarColisionConItems(); // Añade esta línea aquí

                this.Invalidate(); // Redibujar la ventana para actualizar la posición de los bots
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en TimerBots_Tick: {ex.Message}\nStack Trace: {ex.StackTrace}");
            }
        }

        private Nodo MoverBotEnDireccion(Nodo posicionActual, int direccion)
        {
            if (posicionActual == null || _grid == null)
            {
                Console.WriteLine("Error: posicionActual o _grid es null en MoverBotEnDireccion");
                return null;
            }

            try
            {
                switch (direccion)
                {
                    case 0: // Arriba
                        return _grid.ObtenerNodo(posicionActual.X - 1 < 0 ? _grid.Filas - 1 : posicionActual.X - 1, posicionActual.Y);
                    case 1: // Derecha
                        return _grid.ObtenerNodo(posicionActual.X, posicionActual.Y + 1 >= _grid.Columnas ? 0 : posicionActual.Y + 1);
                    case 2: // Abajo
                        return _grid.ObtenerNodo(posicionActual.X + 1 >= _grid.Filas ? 0 : posicionActual.X + 1, posicionActual.Y);
                    case 3: // Izquierda
                        return _grid.ObtenerNodo(posicionActual.X, posicionActual.Y - 1 < 0 ? _grid.Columnas - 1 : posicionActual.Y - 1);
                    default:
                        Console.WriteLine($"Dirección inválida en MoverBotEnDireccion: {direccion}");
                        return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en MoverBotEnDireccion: {ex.Message}");
                return null;
            }
        }

        private float DireccionARotacion(int direccion)
        {
            // Convertir la dirección en un ángulo de rotación
            switch (direccion)
            {
                case 0: // Arriba
                    return 0;
                case 1: // Derecha
                    return 90;
                case 2: // Abajo
                    return 180;
                case 3: // Izquierda
                    return -90;
                default:
                    return 0;
            }
        }

        private bool EsPosicionOcupadaPorOtroBot(Nodo posicion)
        {
            // Verificar si la posición está ocupada por otro bot
            for (int i = 0; i < _posicionesBots.Length; i++)
            {
                if (_posicionesBots[i] != null && _posicionesBots[i].Equals(posicion))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
