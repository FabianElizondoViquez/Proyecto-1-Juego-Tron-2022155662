using System.Drawing;
using System.Windows.Forms;

namespace Proyecto1JuegoTron
{
    public partial class Form1 : Form
    {
        private void CargarMoto()
        {
            try
            {
                _motoAzul = Image.FromFile("motoazul.png");
                _anchoMoto = 35;  // Ancho de la moto
                _altoMoto = 40;   // Alto de la moto
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la imagen de la moto: " + ex.Message);
            }
        }

        private void DibujarMoto(Graphics g)
        {
            if (_posicionMoto != null && _motoAzul != null)
            {
                float xCentro = _posicionMoto.Y * _tamañoNodo + _tamañoNodo / 2.0f;
                float yCentro = _posicionMoto.X * _tamañoNodo + _tamañoNodo / 2.0f;

                g.TranslateTransform(xCentro, yCentro);
                g.RotateTransform(_anguloRotacion);
                g.TranslateTransform(-xCentro, -yCentro);

                g.DrawImage(_motoAzul, new Rectangle(
                    (int)(xCentro - _anchoMoto / 2),
                    (int)(yCentro - _altoMoto / 2),
                    _anchoMoto,
                    _altoMoto));
            }
        }
    }
}
