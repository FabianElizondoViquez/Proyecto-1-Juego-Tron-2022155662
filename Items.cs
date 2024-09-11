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

        private void InicializarItems()
        {
            _colaItems = new Queue<Item>();
            _itemsEnGrid = new List<Item>();
            _procesandoItems = false;
            _gridAncho = 20; // Ajusta según el tamaño de tu grid
            _gridAlto = 15;
            ConfigurarTimerItems();
        }

        private void ConfigurarTimerItems()
        {
            _timerCrearItems = new System.Windows.Forms.Timer();
            _timerCrearItems.Interval = 5000; // Crear un item cada 5 segundos
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
            _colaItems.Enqueue(nuevoItem);
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

        private async void ProcesarItems()
        {
            _procesandoItems = true;

            while (_colaItems.Count > 0)
            {
                Item itemActual = _colaItems.Dequeue();
                await Task.Delay(1000); // Delay de 1 segundo entre la aplicación de los items

                switch (itemActual.Tipo)
                {
                    case TipoItem.CeldaCombustible:
                        if (_combustibleMoto < 100)
                        {
                            _combustibleMoto += itemActual.Valor;
                            if (_combustibleMoto > 100)
                                _combustibleMoto = 100; // Límite máximo del combustible
                        }
                        else
                        {
                            _colaItems.Enqueue(itemActual); // Reinsertar si el combustible está lleno
                        }
                        break;

                    case TipoItem.CrecimientoEstela:
                        int incremento = itemActual.Valor;
                        for (int i = 0; i < incremento; i++)
                        {
                            _estelaMoto.AgregarNodo(_posicionMoto); // Aumentar estela
                        }
                        break;

                    case TipoItem.Bomba:
                        FinDelJuego(); // Terminar el juego
                        break;
                }

                _infoPanel.Invalidate(); // Redibujar la información en pantalla
            }

            _procesandoItems = false;
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
                        AplicarEfectoItem(item);
                        this.Controls.Remove(picBox);
                        _itemsEnGrid.Remove(item);
                        break;
                    }
                }
            }
        }

        private void AplicarEfectoItem(Item item)
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
                    for (int i = 0; i < item.Valor; i++)
                    {
                        _estelaMoto.AgregarNodo(_posicionMoto);
                    }
                    break;

                case TipoItem.Bomba:
                    FinDelJuego(); // Terminar el juego
                    MessageBox.Show("¡Bomba! La moto explotó.");
                    break;
            }

            _infoPanel.Invalidate();
        }

        private void ActualizarJuego()
        {
            // ... código para mover la moto ...

            VerificarColisionConItems();

            // ... resto de la lógica del juego ...
        }
    }
}
