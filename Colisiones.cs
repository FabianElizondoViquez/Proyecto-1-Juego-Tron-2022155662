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
            // Verificar colisión con estelas de los bots
            foreach (var estela in _estelasBots)
            {
                if (estela.Any(nodo => nodo != null && nodo.X == _posicionMoto.X && nodo.Y == _posicionMoto.Y))
                {
                    FinDelJuego();
                    return;
                }
            }

            // Verificar colisión con los bots
            if (_posicionesBots.Any(bot => bot != null && bot.X == _posicionMoto.X && bot.Y == _posicionMoto.Y))
            {
                FinDelJuego();
                return;
            }
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
