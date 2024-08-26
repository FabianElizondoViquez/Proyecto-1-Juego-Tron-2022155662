using System;
using System.Windows.Forms;

namespace Proyecto1JuegoTron
{
    public partial class Form1 : Form
    {
        private void Form1_Resize(object sender, EventArgs e)
        {
            CrearGrid();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    _anguloRotacion = 0;
                    break;
                case Keys.Down:
                    _anguloRotacion = 180;
                    break;
                case Keys.Left:
                    _anguloRotacion = -90;
                    break;
                case Keys.Right:
                    _anguloRotacion = 90;
                    break;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Nodo nuevoNodo = null;

            switch (_anguloRotacion)
            {
                case 0:
                    nuevoNodo = _posicionMoto.Arriba;
                    break;
                case 180:
                    nuevoNodo = _posicionMoto.Abajo;
                    break;
                case -90:
                    nuevoNodo = _posicionMoto.Izquierda;
                    break;
                case 90:
                    nuevoNodo = _posicionMoto.Derecha;
                    break;
            }

            if (nuevoNodo != null)
            {
                Estela nuevaEstela = new Estela(_posicionMoto);
                nuevaEstela.Siguiente = _estelaMoto;
                _estelaMoto = nuevaEstela;

                Estela tempEstela = _estelaMoto;
                int count = 0;
                while (tempEstela != null && count < 3)
                {
                    tempEstela = tempEstela.Siguiente;
                    count++;
                }
                if (tempEstela != null)
                {
                    tempEstela.Siguiente = null;
                }

                _posicionMoto = nuevoNodo;
                this.Invalidate();
            }
        }
    }
}
