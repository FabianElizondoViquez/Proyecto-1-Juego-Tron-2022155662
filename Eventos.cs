using System;
using System.Windows.Forms;

namespace Proyecto1JuegoTron
{
    /// <summary>
    /// Parte de la clase `Form1` que maneja los eventos clave del juego, incluyendo redimensionamiento de la ventana, control del teclado y actualización del estado del juego.
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// Contador de los nodos recorridos por la moto en el grid.
        /// </summary>
        public int _nodosRecorridos;

        /// <summary>
        /// Evento que se dispara cuando se redimensiona la ventana del formulario.
        /// Vuelve a crear el grid para ajustarse al nuevo tamaño de la ventana.
        /// </summary>
        private void Form1_Resize(object sender, EventArgs e)
        {
            CrearGrid();
        }

        /// <summary>
        /// Evento que se dispara cuando se presiona una tecla en el formulario.
        /// Actualiza la dirección de movimiento de la moto según la tecla presionada.
        /// </summary>
        /// <param name="sender">El objeto que envía el evento.</param>
        /// <param name="e">Los datos del evento, que contienen la tecla presionada.</param>
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

        /// <summary>
        /// Evento que se ejecuta en cada tick del temporizador, actualizando la posición de la moto,
        /// la estela que deja, y verificando el estado del combustible.
        /// </summary>
        /// <param name="sender">El objeto que envía el evento.</param>
        /// <param name="e">Los datos del evento.</param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Verificar si queda combustible
            if (_combustibleMoto > 0)
            {
                Nodo nuevoNodo = null;

                // Determinar el nuevo nodo basado en la dirección actual de la moto
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

                // Si el nuevo nodo es válido, actualizar la posición de la moto
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
                        _timerMoto.Stop();
                        MessageBox.Show("La moto se ha quedado sin combustible.");
                    }

                    // Solicitar la redibujado de la pantalla para reflejar los cambios
                    this.Invalidate();
                }
            }
        }
    }
}
