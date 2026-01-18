using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System;
using AlarmaSueño.Core;

namespace AlarmaSueño
{
    public partial class ConfirmationDialog : Form
    {
        public ConfirmationDialog(string message)
        {
            InitializeComponent();
            lblMessage.Text = message;

            // Aplicar estilos
            this.BackColor = ColorTranslator.FromHtml("#023047"); // Color de fondo principal
            lblMessage.ForeColor = Color.White;
            lblMessage.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);

            // Set localized texts
            this.Text = I18n.GetString("ConfirmationDialogTitle");
            btnOk.Text = I18n.GetString("ButtonOkText");

            // Cargar el icono de la aplicación desde recursos
            this.Icon = ResourceLoader.LoadIcon("icon.ico");

            // Centrar el formulario en la pantalla
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void btnOk_Click(object? sender, EventArgs e)
        {
            this.Close();
        }
    }
}
