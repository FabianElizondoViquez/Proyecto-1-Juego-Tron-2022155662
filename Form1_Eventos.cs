using System;
using System.Windows.Forms;

namespace Proyecto1JuegoTron
{
    public partial class Form1 : Form
    {
        public int _nodosRecorridos;

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
            // Verificar si queda combustible
            if (_combustibleMoto > 0)
            {
                _combustibleMoto -= _velocidadMoto / 10; // Ajustar la tasa de consumo
                _combustibleMoto = Math.Max(_combustibleMoto, 0); // Evitar valores negativos
            }
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
                    // Agregar la nueva posición a la estela
                    Estela nuevaEstela = new Estela(_posicionMoto);
                    nuevaEstela.Siguiente = _estelaMoto;
                    _estelaMoto = nuevaEstela;

                    // Limitar la estela a las últimas 3 posiciones
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

                    // Actualizar la posición de la moto
                    _posicionMoto = nuevoNodo;

                    // Incrementar el contador de nodos recorridos
                    _nodosRecorridos++;
                    
                    // Reducir el combustible cada 5 nodos recorridos
                    if (_nodosRecorridos % 5 == 0)
                    {
                        _combustibleMoto--;
                    }

                    // Verificar si el combustible se agotó
                    if (_combustibleMoto <= 0)
                    {
                        _combustibleMoto = 0;
                        _timer.Stop();
                        MessageBox.Show("La moto se ha quedado sin combustible.");
                    }
                    
                    this.Invalidate(); // Redibujar la pantalla
                
                }
            }
        }
    }
}

