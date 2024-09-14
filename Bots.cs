using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Proyecto1JuegoTron
{
    public partial class Form1 : Form
    {
        private Image _motoAmarilla;                   
        private Nodo[] _posicionesBots;                 
        private int[] _direccionesBots;                 
        private int[] _pasosRestantesBots;              
        private float[] _angulosRotacionBots;           
        private Random _randomBot;                     
        private System.Windows.Forms.Timer _timerBots; 
        private List<Nodo>[] _estelasBots;             
        private System.Windows.Forms.Timer _timerNuevoBot; 
        private const int TIEMPO_NUEVO_BOT = 60000; 

        private void InicializarBots()
        {
            _posicionesBots = new Nodo[4];              
            _direccionesBots = new int[4];              
            _pasosRestantesBots = new int[4];           
            _angulosRotacionBots = new float[4];        
            _estelasBots = new List<Nodo>[4];           
            _randomBot = new Random();
            try
            {
                _motoAmarilla = Image.FromFile("motoamarilla.png");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la imagen de la moto amarilla: " + ex.Message);
            }

            for (int i = 0; i < _posicionesBots.Length; i++)
            {
                _posicionesBots[i] = _grid.ObtenerNodo(_randomBot.Next(0, _grid.Filas), _randomBot.Next(0, _grid.Columnas));
                _direccionesBots[i] = _randomBot.Next(0, 4); 
                _pasosRestantesBots[i] = _randomBot.Next(5, 15); 
                _angulosRotacionBots[i] = DireccionARotacion(_direccionesBots[i]); 
                _estelasBots[i] = new List<Nodo>(); 
            }

            _velocidadBot = _randomBot.Next(1, 11); 
            _timerBots = new System.Windows.Forms.Timer();
            _timerBots.Interval = 500 / _velocidadBot; 
            _timerBots.Tick += new EventHandler(TimerBots_Tick);
            _timerBots.Start();

            _timerNuevoBot = new System.Windows.Forms.Timer();
            _timerNuevoBot.Interval = TIEMPO_NUEVO_BOT;
            _timerNuevoBot.Tick += new EventHandler(TimerNuevoBot_Tick);
            _timerNuevoBot.Start();
        }

        private void DibujarBots(Graphics g)
        {
            if (_posicionesBots != null && _motoAmarilla != null)
            {
                for (int i = 0; i < _posicionesBots.Length; i++)
                {
                    if (_posicionesBots[i] != null)
                    {
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

                        float xCentro = _posicionesBots[i].Y * _tamañoNodo + _tamañoNodo / 2.0f;
                        float yCentro = _posicionesBots[i].X * _tamañoNodo + _tamañoNodo / 2.0f;
                        g.TranslateTransform(xCentro, yCentro);       
                        g.RotateTransform(_angulosRotacionBots[i]);  
                        g.TranslateTransform(-xCentro, -yCentro);     
                        g.DrawImage(_motoAmarilla, new Rectangle(
                            (int)(xCentro - _anchoMoto / 2),
                            (int)(yCentro - _altoMoto / 2),
                            _anchoMoto,
                            _altoMoto));
                        g.ResetTransform(); 
                    }
                }
            }
        }

        private void TimerBots_Tick(object sender, EventArgs e)
        {
            try
            {
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
                    if (_estelasBots[i] != null)
                    {
                        _estelasBots[i].Insert(0, _posicionesBots[i]);

                        if (_estelasBots[i].Count > 3)
                        {
                            _estelasBots[i].RemoveAt(_estelasBots[i].Count - 1);
                        }
                    }

                    if (nuevasPosiciones[i] != null && !EsPosicionOcupadaPorOtroBot(nuevasPosiciones[i]))
                    {
                        _posicionesBots[i] = nuevasPosiciones[i];
                        
                        if (--_pasosRestantesBots[i] <= 0)
                        {
                            _direccionesBots[i] = _randomBot.Next(0, 4); 
                            _angulosRotacionBots[i] = DireccionARotacion(_direccionesBots[i]); 
                            _pasosRestantesBots[i] = _randomBot.Next(5, 15); 
                        }
                    }
                    else
                    {
                        _direccionesBots[i] = _randomBot.Next(0, 4);
                        _angulosRotacionBots[i] = DireccionARotacion(_direccionesBots[i]);
                    }
                }

                VerificarColisiones(); 
                VerificarColisionConItems(); 

                this.Invalidate(); 
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
            for (int i = 0; i < _posicionesBots.Length; i++)
            {
                if (_posicionesBots[i] != null && _posicionesBots[i].Equals(posicion))
                {
                    return true;
                }
            }
            return false;
        }

        private void TimerNuevoBot_Tick(object sender, EventArgs e)
        {
            AgregarNuevoBot();
            _timerNuevoBot.Stop(); 
        }

        private void AgregarNuevoBot()
        {
            Array.Resize(ref _posicionesBots, _posicionesBots.Length + 1);
            Array.Resize(ref _direccionesBots, _direccionesBots.Length + 1);
            Array.Resize(ref _pasosRestantesBots, _pasosRestantesBots.Length + 1);
            Array.Resize(ref _angulosRotacionBots, _angulosRotacionBots.Length + 1);
            Array.Resize(ref _estelasBots, _estelasBots.Length + 1);

            int nuevoIndice = _posicionesBots.Length - 1;

            _posicionesBots[nuevoIndice] = _grid.ObtenerNodo(_randomBot.Next(0, _grid.Filas), _randomBot.Next(0, _grid.Columnas));
            _direccionesBots[nuevoIndice] = _randomBot.Next(0, 4);
            _pasosRestantesBots[nuevoIndice] = _randomBot.Next(5, 15);
            _angulosRotacionBots[nuevoIndice] = DireccionARotacion(_direccionesBots[nuevoIndice]);
            _estelasBots[nuevoIndice] = new List<Nodo>();

            Console.WriteLine($"Nuevo bot agregado. Total de bots: {_posicionesBots.Length}");
        }
    }
}
