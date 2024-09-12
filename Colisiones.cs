using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Proyecto1JuegoTron
{
    public partial class Form1 : Form
    {
        private void VerificarColisiones()
        {
            try
            {
                // Verificar colisión de la moto del jugador con estelas de los bots
                for (int i = 0; i < _posicionesBots.Length; i++)
                {
                    if (_posicionesBots[i] != null && _estelasBots[i] != null)
                    {
                        if (_estelasBots[i].Any(nodo => nodo != null && nodo.X == _posicionMoto.X && nodo.Y == _posicionMoto.Y))
                        {
                            FinDelJuego();
                            return;
                        }
                    }
                }

                // Verificar colisión de la moto del jugador con los bots
                if (_posicionesBots.Any(bot => bot != null && bot.X == _posicionMoto.X && bot.Y == _posicionMoto.Y))
                {
                    FinDelJuego();
                    return;
                }

                // Verificar colisión de los bots con la estela del jugador
                Estela estelaJugador = _estelaMoto;
                while (estelaJugador != null)
                {
                    for (int i = 0; i < _posicionesBots.Length; i++)
                    {
                        if (_posicionesBots[i] != null && 
                            _posicionesBots[i].X == estelaJugador.Nodo.X && 
                            _posicionesBots[i].Y == estelaJugador.Nodo.Y)
                        {
                            Console.WriteLine($"Colisión detectada: Bot {i} en ({_posicionesBots[i].X}, {_posicionesBots[i].Y})");
                            EliminarBot(i);
                        }
                    }
                    estelaJugador = estelaJugador.Siguiente;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en VerificarColisiones: {ex.Message}\nStack Trace: {ex.StackTrace}");
            }
        }

        private void EliminarBot(int indice)
        {
            Console.WriteLine($"Eliminando Bot {indice}");
            _posicionesBots[indice] = null;
            if (_estelasBots[indice] != null)
            {
                _estelasBots[indice].Clear();
            }
            Console.WriteLine($"Bot {indice} eliminado");
            this.Invalidate(); // Redibujar el formulario
        }

        private void FinDelJuego()
        {
            // Detener los timers de la moto y los bots
            _timerMoto.Stop();
            if (_timerBots != null)
            {
                _timerBots.Stop();
            }

            // Mostrar mensaje de fin del juego
            MessageBox.Show("Has perdido", "Juego Terminado", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Opcional: Cerrar la aplicación o reiniciar el juego
            Application.Exit();
        
        }
    }
}
