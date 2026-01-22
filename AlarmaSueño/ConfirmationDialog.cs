using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System;
using AlarmaSueño.Core;

namespace AlarmaSueño
{
    public partial class ConfirmationDialog : Form
    {
        public ConfirmationDialog(string message, bool isYesNo = false)
        {
            InitializeComponent();
            lblMessage.Text = message;

            // Apply styles
            this.BackColor = ColorTranslator.FromHtml("#023047"); // Main background color
            lblMessage.ForeColor = Color.White;
            lblMessage.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            
            this.Text = I18n.GetString("ConfirmationDialogTitle");

            if (isYesNo)
            {
                btnYes.Text = I18n.GetString("ButtonYesText", "Sí"); // Assuming ButtonYesText exists or falls back
                btnNo.Text = I18n.GetString("ButtonNoText", "No"); // Assuming ButtonNoText exists or falls back
                btnNo.Visible = true;
            }
            else
            {
                btnYes.Text = I18n.GetString("ButtonOkText");
                btnNo.Visible = false;
                // Center the single button
                btnYes.Location = new Point((this.ClientSize.Width - btnYes.Width) / 2, btnYes.Location.Y);
            }

            this.Icon = ResourceLoader.LoadIcon("icon.ico");
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void btnYes_Click(object? sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes; // Also works for OK
            this.Close();
        }

        private void btnNo_Click(object? sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }
    }
}
