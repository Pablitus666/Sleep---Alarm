using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using AlarmaSueño.Core;

namespace AlarmaSueño
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();

            // Asignar textos desde el archivo de cadenas centralizado
            this.Text = I18n.GetString("AboutFormTitle");
            this.labelInfo.Text = I18n.GetString("AboutFormInfoLabel");
            this.btnCerrar.Text = I18n.GetString("AboutFormCloseButton");

            if (pictureBoxRobot != null)
            {
                // Cargar la imagen del robot directamente para aislar el problema.
                // PictureBox gestionará la disposición de la imagen asignada.
                pictureBoxRobot.Image = ResourceLoader.LoadImage("robot.png");
            }

            // Establecer icono desde recursos
            Icon? appIcon = ResourceLoader.LoadIcon("icon.ico");
            if (appIcon != null)
            {
                this.Icon = appIcon;
            }
        }

        private void btnCerrar_Click(object? sender, EventArgs e)
        {
            this.Close();
        }
    }
}
