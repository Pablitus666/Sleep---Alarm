using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlarmaSueño.Core;
using Microsoft.Extensions.DependencyInjection;

namespace AlarmaSueño
{
    public partial class MainForm : Form
    {
        private readonly IAlarmManager _alarmManager;
        private readonly IPhraseProvider _phraseProvider;
        private readonly IAudioPlayer _audioPlayer;
        private readonly ISettingsManager _settingsManager;
        private readonly TrayIconManager _trayIconManager; 
        
        private System.Windows.Forms.Timer? _snoozeTimer;
        private Dictionary<string, Image>? _logoVariations;

        // --- Controles de UI ---
        private DateTimePicker? timePicker;
        private PressableImageButton? btnGuardarAlarma;
        private Label? lblHoraActual;
        private TableLayoutPanel? panelConfig; // Panel para la vista de configuración

        private int? _currentPostponeMinutes;

        public MainForm(IAlarmManager alarmManager, IPhraseProvider phraseProvider, IAudioPlayer audioPlayer, ISettingsManager settingsManager)
        {
            InitializeComponent();
            
            _alarmManager = alarmManager;
            _phraseProvider = phraseProvider;
            _audioPlayer = audioPlayer;
            _settingsManager = settingsManager;
            
            _alarmManager.AlarmTriggered += OnAlarmTriggered;

            Icon? appIcon = ResourceLoader.LoadIcon("icon.ico");
            if (appIcon != null)
            {
                this.Icon = appIcon;
            }

            _trayIconManager = new TrayIconManager(this.Icon ?? new Icon(SystemIcons.Application, 48, 48));
            _trayIconManager.ShowApplicationRequested += (s, e) => ShowWindow();
            _trayIconManager.ExitApplicationRequested += (s, e) => Application.Exit();

            InitializeEnhancedLogo();
            InitializeCustomControls();
            
            LoadDataAsync();

            this.FormClosing += MainForm_FormClosing;
            chkAutoStart.CheckedChanged += chkAutoStart_CheckedChanged;
            chkLockSettings.CheckedChanged += chkLockSettings_CheckedChanged;
        }

        private async void LoadDataAsync()
        {
            await _phraseProvider.LoadQuotesAsync();
            AppSettings settings = await _settingsManager.LoadSettingsAsync();

            if (timePicker != null) timePicker.Value = settings.AlarmTime;
            
            _alarmManager.SetAlarmTime(settings.AlarmTime);
            UpdateAlarmTimeLabel(settings.AlarmTime);
            
            chkLockSettings.Checked = settings.IsLocked;
            ApplyLockState(settings.IsLocked);

            if (settings.AutoStart && !WindowsIntegration.IsStartupEntryExist())
            {
                WindowsIntegration.AddStartupEntry();
            }
            else if (!settings.AutoStart && WindowsIntegration.IsStartupEntryExist())
            {
                WindowsIntegration.RemoveStartupEntry();
            }
            chkAutoStart.Checked = settings.AutoStart;

            SwitchToConfigurationView();

            _snoozeTimer = new System.Windows.Forms.Timer { Interval = settings.SnoozeMinutes * 60 * 1000 };
            _snoozeTimer.Tick += _snoozeTimer_Tick;

            _currentPostponeMinutes = settings.PostponeMinutes;
            
            _alarmManager.Start();

            InitializeStrings();
        }

        private void InitializeStrings()
        {
            this.Text = I18n.GetString("MainFormTitle");
            lblFrase.Text = I18n.GetString("MotivationPhraseDefault");
            btnCerrar.Text = I18n.GetString("CloseButtonText");
            btnPosponer.Text = I18n.GetString("PostponeButtonText", _currentPostponeMinutes ?? 10);
            chkAutoStart.Text = I18n.GetString("StartWithWindowsCheckbox");
            chkLockSettings.Text = I18n.GetString("LockSettingsCheckbox");
        }

        private void InitializeEnhancedLogo()
        {
            if (pictureBoxLogo == null) return;
            using (Image? originalLogo = ResourceLoader.LoadImage("logo.png"))
            {
                if (originalLogo != null)
                {
                    Color glowColor = ColorTranslator.FromHtml("#fcbf49");
                    _logoVariations = ImageEnhancer.CreateLogoVariations(originalLogo, pictureBoxLogo.Size, glowColor);
                    pictureBoxLogo.Image = _logoVariations["normal"];
                    pictureBoxLogo.MouseEnter += PictureBoxLogo_MouseEnter;
                    pictureBoxLogo.MouseLeave += PictureBoxLogo_MouseLeave;
                }
            }
        }
        
        private void PictureBoxLogo_MouseEnter(object? sender, EventArgs e)
        {
            if (_logoVariations?["hover"] != null && pictureBoxLogo != null) pictureBoxLogo.Image = _logoVariations["hover"];
        }

        private void PictureBoxLogo_MouseLeave(object? sender, EventArgs e)
        {
            if (_logoVariations?["normal"] != null && pictureBoxLogo != null) pictureBoxLogo.Image = _logoVariations["normal"];
        }

        private void pictureBoxLogo_Click(object? sender, EventArgs e)
        {
            using (var aboutForm = new AboutForm())
            {
                aboutForm.ShowDialog(this);
            }
        }
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.Hide();
            this.ShowInTaskbar = false;
        }

        private void InitializeCustomControls()
        {
            panelConfig = new TableLayoutPanel { Dock = System.Windows.Forms.DockStyle.Fill, ColumnCount = 1 };
            panelConfig.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            panelConfig.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            panelConfig.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            panelConfig.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            lblHoraActual = new Label { Anchor = AnchorStyles.None, ForeColor = ColorTranslator.FromHtml("#A1D6E2"), Font = new Font("Segoe UI", 14F, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter, BackColor = Color.Transparent, AutoSize = true };
            panelConfig.Controls.Add(lblHoraActual, 0, 0);

            timePicker = new DateTimePicker { Anchor = AnchorStyles.None, Format = DateTimePickerFormat.Time, ShowUpDown = true, Size = new Size(250, 40), Font = new Font("Segoe UI", 16F, FontStyle.Bold), Margin = new Padding(10) };
            panelConfig.Controls.Add(timePicker, 0, 1);

            btnGuardarAlarma = new PressableImageButton { Anchor = AnchorStyles.None, Size = new Size(280, 60), Text = I18n.GetString("ButtonSaveAlarmText"), ImageNormal = ResourceLoader.LoadImage("boton.png"), Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = Color.White };
            btnGuardarAlarma.Click += btnGuardarAlarma_Click;
            panelConfig.Controls.Add(btnGuardarAlarma, 0, 2);
            
            this.AcceptButton = btnGuardarAlarma;
        }

        private async void btnGuardarAlarma_Click(object? sender, EventArgs e)
        {
            if (timePicker == null) return;
            DateTime selectedTime = timePicker.Value;
            _alarmManager.SetAlarmTime(selectedTime);
            UpdateAlarmTimeLabel(selectedTime);

            AppSettings settingsToSave = await _settingsManager.LoadSettingsAsync(); 
            settingsToSave.AlarmTime = selectedTime;
            settingsToSave.IsLocked = chkLockSettings.Checked;
            settingsToSave.AutoStart = chkAutoStart.Checked;
            await _settingsManager.SaveSettingsAsync(settingsToSave);

            using (var dialog = new ConfirmationDialog(I18n.GetString("AlarmSavedMessage", selectedTime)))
            {
                dialog.ShowDialog();
            }
            this.Hide(); 
        }

        private void UpdateAlarmTimeLabel(DateTime time)
        {
            if (lblHoraActual != null) lblHoraActual.Text = I18n.GetString("AlarmTimeDisplayMessage", time);
        }

        private void OnAlarmTriggered(object? sender, EventArgs e)
        {
            Invoke(new Action(() =>
            {
                SwitchToAlarmView();
                _audioPlayer.PlayAlarmSound();
                ShowWindow();
            }));
        }

        private void SwitchToConfigurationView(bool hideForm = true)
        {
            // Restaurar el margen de la frase a su estado original
            lblFrase.Margin = new Padding(0);

            // Restaurar los estilos de fila a su estado original para la vista de configuración
            tableLayoutPanelMain.RowStyles[1].SizeType = SizeType.Percent;
            tableLayoutPanelMain.RowStyles[1].Height = 100F;
            tableLayoutPanelMain.RowStyles[2].SizeType = SizeType.AutoSize;
            tableLayoutPanelMain.RowStyles[3].SizeType = SizeType.AutoSize;
            
            // Intercambiar controles
            tableLayoutPanelMain.Controls.Remove(lblFrase);
            tableLayoutPanelMain.Controls.Remove(tableLayoutPanelAlarmButtons);
            if (panelConfig != null) tableLayoutPanelMain.Controls.Add(panelConfig, 0, 1);

            // Restaurar visibilidad de los checkboxes
            chkAutoStart.Visible = true;
            chkLockSettings.Visible = true;
            if (hideForm) this.Hide();
        }

        private void SwitchToAlarmView()
        {
            if (panelConfig != null) tableLayoutPanelMain.Controls.Remove(panelConfig);

            // --- Configuración de Layout para la Vista de Alarma ---
            
            // 1. Establecer margen superior de la frase para crear el espacio de 50px
            lblFrase.Margin = new Padding(3, 23, 3, 0);

            // 2. Configurar las filas para un diseño estable y predecible
            tableLayoutPanelMain.RowStyles[1].SizeType = SizeType.Percent;   // La fila de la frase absorberá el espacio extra
            tableLayoutPanelMain.RowStyles[1].Height = 100F;
            tableLayoutPanelMain.RowStyles[2].SizeType = SizeType.AutoSize;  // La fila de los botones se ajustará a su contenido
            tableLayoutPanelMain.RowStyles[3].SizeType = SizeType.Absolute;  // La última fila será el padding inferior fijo
            tableLayoutPanelMain.RowStyles[3].Height = 18F;

            // 3. Añadir los controles a sus filas designadas
            tableLayoutPanelMain.Controls.Add(lblFrase, 0, 1);
            tableLayoutPanelMain.Controls.Add(tableLayoutPanelAlarmButtons, 0, 2);

            // Ocultar controles de configuración
            chkAutoStart.Visible = false;
            chkLockSettings.Visible = false;

            lblFrase.Text = _phraseProvider.ObtenerFrase();
        }

        private async void chkAutoStart_CheckedChanged(object? sender, EventArgs e)
        {
            try
            {
                if (chkAutoStart.Checked) WindowsIntegration.AddStartupEntry();
                else WindowsIntegration.RemoveStartupEntry();
                await SaveAllCurrentSettingsFromUiAsync();
            }
            catch (Exception ex)
            {
                Program.ServiceProvider?.GetService<ILogger>()?.LogException(ex);
                MessageBox.Show(string.Format(I18n.GetString("AutoStartConfigErrorMessage"), ex.Message), I18n.GetString("ErrorTitle"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                chkAutoStart.Checked = !chkAutoStart.Checked;
            }
        }

        private async Task SaveAllCurrentSettingsFromUiAsync()
        {
            if (timePicker == null) return;
            AppSettings settingsToSave = await _settingsManager.LoadSettingsAsync(); 
            settingsToSave.AlarmTime = timePicker.Value;
            settingsToSave.IsLocked = chkLockSettings.Checked;
            settingsToSave.AutoStart = chkAutoStart.Checked;
            await _settingsManager.SaveSettingsAsync(settingsToSave);
        }

        private async void chkLockSettings_CheckedChanged(object? sender, EventArgs e)
        {
            if (timePicker == null) return;
            ApplyLockState(chkLockSettings.Checked);
            await SaveAllCurrentSettingsFromUiAsync();
        }

        private void ApplyLockState(bool isLocked)
        {
            if (timePicker != null) timePicker.Enabled = !isLocked;
        }
        
        private void _snoozeTimer_Tick(object? sender, EventArgs e)
        {
            if(_snoozeTimer != null) _snoozeTimer.Stop();
            OnAlarmTriggered(sender, e);
        }

        private void ShowWindow()
        {
            this.Show();
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
        }

        private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    _trayIconManager?.Dispose();
                    _snoozeTimer?.Dispose();
                    _audioPlayer?.Dispose();
                    components?.Dispose();
                    if (_logoVariations != null)
                    {
                        var logoVariations = _logoVariations; 
                        foreach (var entry in logoVariations.Values) entry.Dispose();
                        logoVariations.Clear();
                        _logoVariations = null;
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        private void btnCerrar_Click(object? sender, EventArgs e)
        {
            _audioPlayer.StopAlarmSound();
            using (var dialog = new ConfirmationDialog(I18n.GetString("SleepWellMessage")))
            {
                dialog.ShowDialog();
            }
            SwitchToConfigurationView();
        }

        private void btnPosponer_Click(object? sender, EventArgs e)
        {
            _audioPlayer.StopAlarmSound();
            if (_currentPostponeMinutes.HasValue)
            {
                _alarmManager.Posponer(_currentPostponeMinutes.Value);
                using (var dialog = new ConfirmationDialog(I18n.GetString("AlarmPostponedMessage", _currentPostponeMinutes.Value)))
                {
                    dialog.ShowDialog();
                }
            }
            else
            {
                _alarmManager.Posponer(10);
                using (var dialog = new ConfirmationDialog(I18n.GetString("AlarmPostponedFallbackMessage")))
                {
                    dialog.ShowDialog();
                }
            }
            SwitchToConfigurationView();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Program.WM_SHOWME)
            {
                ShowWindow();
            }
            base.WndProc(ref m);
        }
    }
}
