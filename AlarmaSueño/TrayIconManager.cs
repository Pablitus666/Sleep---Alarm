using System;
using System.Drawing;
using System.Windows.Forms;
using AlarmaSueño.Core; // For I18n

namespace AlarmaSueño
{
    public class TrayIconManager : IDisposable
    {
        private readonly NotifyIcon _notifyIcon;
        private readonly ContextMenuStrip _contextMenu;

        // Eventos para comunicar acciones a la ventana principal
        public event EventHandler? ShowApplicationRequested;
        public event EventHandler? ExitApplicationRequested;

        public TrayIconManager(Icon appIcon)
        {
            _contextMenu = new ContextMenuStrip();
            _contextMenu.Items.Add(I18n.GetString("TrayIconShowText"), null, OnShowApplication);
            _contextMenu.Items.Add(I18n.GetString("TrayIconExitText"), null, OnExitApplication);

            _notifyIcon = new NotifyIcon
            {
                Icon = appIcon,
                Text = I18n.GetString("AppDisplayName"),
                ContextMenuStrip = _contextMenu,
                Visible = true
            };

            _notifyIcon.DoubleClick += OnShowApplication;
        }

        private void OnShowApplication(object? sender, EventArgs e)
        {
            // Disparar el evento para que el formulario principal se muestre
            ShowApplicationRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OnExitApplication(object? sender, EventArgs e)
        {
            // Disparar el evento para que la aplicación principal se cierre
            ExitApplicationRequested?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            // Limpiar recursos
            _notifyIcon.Dispose();
            _contextMenu.Dispose();
        }
    }
}
