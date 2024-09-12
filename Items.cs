using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto1JuegoTron
{
    public enum TipoItem
    {
        CeldaCombustible,
        CrecimientoEstela,
        Bomba
    }

    public class Item
    {
        public TipoItem Tipo { get; set; }
        public int Valor { get; set; }
        public int PosicionX { get; set; }
        public int PosicionY { get; set; }

        public Item(TipoItem tipo, int valor)
        {
            Tipo = tipo;
            Valor = valor;
            PosicionX = -1;
            PosicionY = -1;
        }
    }

    public partial class Form1 : Form
    {
        private Queue<Item> _colaItems; // Cola de items para aplicar a la moto
        private bool _procesandoItems;
        private System.Windows.Forms.Timer _timerCrearItems;
        private List<Item> _itemsEnGrid;
        private int _gridAncho, _gridAlto;
        private Random _randomItem;
        private int _timerItemrandom;


        private void InicializarItems()
        {
            _colaItems = new Queue<Item>();
            _itemsEnGrid = new List<Item>();
            _procesandoItems = false;
            _gridAncho = 120; // Ajusta según el tamaño de tu grid
            _gridAlto = 100;
            ConfigurarTimerItems();
        }

        private void ConfigurarTimerItems()
        {
            _randomItem = new Random();
            _timerItemrandom = _randomItem.Next(1000, 10000);
            _timerCrearItems = new System.Windows.Forms.Timer();
            _timerCrearItems.Interval = _timerItemrandom; // Crear un item cada 5 segundos
            _timerCrearItems.Tick += (sender, e) => CrearItemAleatorio();
            _timerCrearItems.Start();
        }

        private void CrearItemAleatorio()
        {
            Random random = new Random();
            TipoItem tipo = (TipoItem)random.Next(0, 3); // 0 = CeldaCombustible, 1 = CrecimientoEstela, 2 = Bomba
            int valor = tipo == TipoItem.CeldaCombustible
                ? random.Next(10, 51) // Las celdas de combustible tienen entre 10 y 50 unidades
                : random.Next(1, 11); // Crecimiento de estela o valor de la bomba
            int gridX = random.Next(0, _gridAncho);
            int gridY = random.Next(0, _gridAlto);

            Item nuevoItem = new Item(tipo, valor);
            nuevoItem.PosicionX = gridX;
            nuevoItem.PosicionY = gridY;
            _itemsEnGrid.Add(nuevoItem);
            DibujarItem(nuevoItem);
        }

        private void DibujarItem(Item item)
        {
            PictureBox picItem = new PictureBox
            {
                Size = new Size(_tamañoNodo, _tamañoNodo), // Cambiar tamaño del item al tamaño del nodo
                Location = new Point(item.PosicionX * _tamañoNodo, item.PosicionY * _tamañoNodo),
                BackColor = ObtenerColorItem(item.Tipo),
                Tag = item
            };

            this.Controls.Add(picItem);
        }

        private Color ObtenerColorItem(TipoItem tipo)
        {
            switch (tipo)
            {
                case TipoItem.CeldaCombustible:
                    return Color.Green;
                case TipoItem.CrecimientoEstela:
                    return Color.Blue;
                case TipoItem.Bomba:
                    return Color.Red;
                default:
                    return Color.Black;
            }
        }

        private void ProcesarYAplicarEfectoItem(Item item)
        {
            switch (item.Tipo)
            {
                case TipoItem.CeldaCombustible:
                    if (_combustibleMoto < 100)
                    {
                        _combustibleMoto += item.Valor;
                        if (_combustibleMoto > 100)
                            _combustibleMoto = 100;
                    }
                    break;

                case TipoItem.CrecimientoEstela:
                    _longitudEstela += item.Valor;
                    Console.WriteLine($"Longitud de estela aumentada a: {_longitudEstela}"); // Mensaje de depuración
                    break;

                case TipoItem.Bomba:
                    _timerMoto.Stop();
                    if (_timerBots != null)
                    {
                        _timerBots.Stop();
                    }

                    MessageBox.Show("¡Bomba! La moto explotó.");
                    FinDelJuego(); // Terminar el juego
                    break;
            }

            _infoPanel.Invalidate();
        }

        private void VerificarColisionConItems()
        {
            Rectangle rectMoto = new Rectangle(_posicionMoto.Y * _tamañoNodo, _posicionMoto.X * _tamañoNodo, _tamañoNodo, _tamañoNodo);

            foreach (Control control in this.Controls)
            {
                if (control is PictureBox picBox && picBox.Tag is Item item)
                {
                    if (rectMoto.IntersectsWith(picBox.Bounds))
                    {
                        this.Controls.Remove(picBox);
                        _itemsEnGrid.Remove(item);
                        ProcesarYAplicarEfectoItem(item); // Aplicar efecto del item a la moto del jugador
                        break;
                    }

                    // Verificar colisión con bots
                    for (int i = 0; i < _posicionesBots.Length; i++)
                    {
                        if (_posicionesBots[i] != null)
                        {
                            Rectangle rectBot = new Rectangle(_posicionesBots[i].Y * _tamañoNodo, _posicionesBots[i].X * _tamañoNodo, _tamañoNodo, _tamañoNodo);
                            if (rectBot.IntersectsWith(picBox.Bounds))
                            {
                                this.Controls.Remove(picBox);
                                _itemsEnGrid.Remove(item);
                                ProcesarYAplicarEfectoItemBot(item, i); // Aplicar efecto del item al bot
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void ProcesarYAplicarEfectoItemBot(Item item, int indiceBot)
        {
            switch (item.Tipo)
            {
                case TipoItem.CeldaCombustible:
                    // Los bots no usan combustible, pero puedes agregar algún efecto si lo deseas
                    break;

                case TipoItem.CrecimientoEstela:
                    // Aumentar la longitud de la estela del bot
                    if (_estelasBots[indiceBot] != null)
                    {
                        for (int j = 0; j < item.Valor; j++)
                        {
                            _estelasBots[indiceBot].Insert(0, _posicionesBots[indiceBot]);
                        }
                    }
                    break;

                case TipoItem.Bomba:
                    // Eliminar el bot
                    EliminarBot(indiceBot);
                    break;
            }
        }

        private void ActualizarJuego()
        {
            // ... código para mover la moto ...

            VerificarColisionConItems();

            // ... resto de la lógica del juego ...
        }
    }
}
