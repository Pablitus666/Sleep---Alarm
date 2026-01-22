using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlarmaSueño.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection; // Added for AudioPlayer Assembly

namespace AlarmaSueño
{
    public partial class MainForm : Form
    {
        private readonly IAlarmManager _alarmManager;
        private readonly IPhraseProvider _phraseProvider;
        private readonly IAudioPlayer _audioPlayer;
        private readonly ISettingsManager _settingsManager;
        private readonly ILogger _logger;
        private readonly TrayIconManager _trayIconManager;

        private Dictionary<string, Image>? _logoVariations;

        private int? _currentPostponeMinutes;
        private readonly bool _isLaunchedByAlarm;
        private bool _hideOnClose = false;
        private bool _isLocked = false;

        public MainForm(IAlarmManager alarmManager, IPhraseProvider phraseProvider, IAudioPlayer audioPlayer, ISettingsManager settingsManager, ILogger logger, string[]? args = null)
        {
            InitializeComponent();

            _alarmManager = alarmManager;
            _phraseProvider = phraseProvider;
            _audioPlayer = audioPlayer;
            _settingsManager = settingsManager;
            _logger = logger;

            _alarmManager.AlarmTriggered += OnAlarmTriggered;

            Icon? appIcon = ResourceLoader.LoadIcon("icon.ico");
            if (appIcon != null)
            {
                this.Icon = appIcon;
            }

            _isLaunchedByAlarm = args != null && Array.Exists(args, arg => arg.Equals("ALARM_TRIGGER", StringComparison.OrdinalIgnoreCase));

            // TrayIconManager setup
            _trayIconManager = new TrayIconManager(this.Icon ?? new Icon(SystemIcons.Application, 48, 48));
            _trayIconManager.ShowApplicationRequested += (s, e) => ShowWindow();
            _trayIconManager.ExitApplicationRequested += (s, e) => Application.Exit();
            
            InitializeEnhancedLogo();
            
            LoadDataAsync(); // Call async method from constructor
            
            this.FormClosing += MainForm_FormClosing;
            this.AcceptButton = btnGuardarAlarma;
            this.timePicker.ValueChanged += new System.EventHandler(this.timePicker_ValueChanged);
        }
        
        private async void LoadDataAsync()
        {
            InitializeStrings();
            await _phraseProvider.LoadQuotesAsync();
            
            // First, update the UI to its current state (it may be snoozing and locked)
            // This ensures all button texts are set before any dialogs appear.
            await UpdateUiForCurrentState();
            
            var settings = await _settingsManager.LoadSettingsAsync();

            // Handle active snooze on startup
            if (settings.SnoozeUntil.HasValue && DateTime.Now < settings.SnoozeUntil.Value && !_isLaunchedByAlarm)
            {
                using (var dialog = new ConfirmationDialog(I18n.GetString("SnoozeActivePrompt"), isYesNo: true))
                {
                    dialog.Text = I18n.GetString("SnoozeActiveTitle");
                    if (dialog.ShowDialog() == DialogResult.Yes)
                    {
                        TerminateOtherRunningInstances();
                        await ClearSnoozeStateAsync();
                        // Now that snooze is cleared, update the UI again to unlock it
                        await UpdateUiForCurrentState();
                    }
                    else
                    {
                        Environment.Exit(0);
                        return;
                    }
                }
            }

            // Re-load settings in case they were cleared
            settings = await _settingsManager.LoadSettingsAsync();
            if (timePicker != null) timePicker.Value = settings.AlarmTime;
            _currentPostponeMinutes = settings.PostponeMinutes;

            if (_isLaunchedByAlarm)
            {
                await ClearSnoozeStateAsync(); // The main alarm is ringing, so snooze is over.
                await UpdateUiForCurrentState(); // Update UI to be interactive again
                SwitchToAlarmView();
                _audioPlayer.PlayAlarmSound();
                ShowWindow();
            }
            else
            {
                // Ensure the correct view is shown on normal startup
                SwitchToConfigurationView(hideForm: false);
            }
            
            _alarmManager.Start();
        }


        private void timePicker_ValueChanged(object? sender, EventArgs e)
        {
            // This handler is intentionally left empty to prevent focus-stealing
            // and allow for a smooth user experience when changing the time.
        }

        private void InitializeStrings()
        {
            this.Text = I18n.GetString("MainFormTitle");
            lblFrase.Text = I18n.GetString("MotivationPhraseDefault");
            btnCerrar.Text = I18n.GetString("CloseButtonText");
            btnPosponer.Text = I18n.GetString("PostponeButtonText", _currentPostponeMinutes ?? 10);
            
            if (btnGuardarAlarma != null) btnGuardarAlarma.Text = I18n.GetString("ButtonSaveAlarmText");
            if (btnDeactivateAlarm != null) btnDeactivateAlarm.Text = I18n.GetString("DeactivateAlarmButtonText");
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
        
        private async void btnGuardarAlarma_Click(object? sender, EventArgs e)
        {
            if (timePicker == null) return;

            await ClearSnoozeStateAsync(); // Saving a new alarm cancels any snooze.

            AppSettings settingsToSave = await _settingsManager.LoadSettingsAsync(); 
            settingsToSave.AlarmTime = timePicker.Value;
            await _settingsManager.SaveSettingsAsync(settingsToSave);

            DateTime selectedTime = timePicker.Value;
            WindowsIntegration.CreateOrUpdateScheduledTask(selectedTime, _logger);
            
            using (var dialog = new ConfirmationDialog(I18n.GetString("AlarmSavedMessage", selectedTime)))
            {
                dialog.ShowDialog();
            }
            Environment.Exit(0);
        }

        private async void btnBloquearAlarma_Click(object? sender, EventArgs e)
        {
            ApplyLockState(!_isLocked); // Toggle the lock state

            var settings = await _settingsManager.LoadSettingsAsync();
            settings.IsLocked = _isLocked;
            await _settingsManager.SaveSettingsAsync(settings);
        }

        private void ApplyLockState(bool isLocked)
        {
            _isLocked = isLocked;
            timePicker.Enabled = !_isLocked;
            btnDeactivateAlarm.Enabled = !_isLocked && WindowsIntegration.IsScheduledTaskActive(_logger);
            
            if (btnBloquearAlarma != null)
            {
                btnBloquearAlarma.Text = _isLocked ? I18n.GetString("UnlockAlarmButtonText") : I18n.GetString("LockAlarmButtonText");
            }
        }

        private async void btnDeactivateAlarm_Click(object? sender, EventArgs e)
        {
            WindowsIntegration.DeleteScheduledTask(_logger);
            await ClearSnoozeStateAsync(); 
            
            var settings = await _settingsManager.LoadSettingsAsync();
            settings.IsLocked = false; // Also unlock if deactivating
            settings.SnoozeUntil = null; // And clear any snooze state
            await _settingsManager.SaveSettingsAsync(settings);
            
            using (var dialog = new ConfirmationDialog(I18n.GetString("AlarmDeactivatedMessage")))
            {
                dialog.ShowDialog();
            }
            
            TerminateOtherRunningInstances();
            Environment.Exit(0);
        }

        private void TerminateOtherRunningInstances()
        {
            Process currentProcess = Process.GetCurrentProcess();
            foreach (Process process in Process.GetProcessesByName(currentProcess.ProcessName))
            {
                if (process.Id != currentProcess.Id)
                {
                    try
                    {
                        process.Kill();
                        _logger.LogInformation($"Terminado proceso AlarmaSueño con PID: {process.Id}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Fallo al terminar proceso con PID: {process.Id}. Excepción: {ex.Message}");
                    }
                }
            }
        }

        private void UpdateAlarmTimeLabel(DateTime time)
        {
            if (lblHoraActual != null) lblHoraActual.Text = I18n.GetString("AlarmTimeDisplayMessage", time);
        }

        private void OnAlarmTriggered(object? sender, EventArgs e)
        {
            // This event can be triggered from a background thread. Marshal to UI thread.
            BeginInvoke(new Action(async () =>
            {
                await ClearSnoozeStateAsync();
                _hideOnClose = false;
                await UpdateUiForCurrentState();
                
                SwitchToAlarmView();
                _audioPlayer.PlayAlarmSound();
                ShowWindow();
            }));
        }

        private void SwitchToConfigurationView(bool hideForm = true)
        {
            this.ClientSize = new System.Drawing.Size(500, 450);

            // Configuration controls
            lblHoraActual.Visible = true;
            timePicker.Visible = true;
            btnGuardarAlarma.Visible = true;
            tableLayoutPanelOnOffButtons.Visible = true;

            // Alarm controls
            lblFrase.Visible = false;
            tableLayoutPanelAlarmButtons.Visible = false;
            
            if (hideForm) this.Hide();
        }

        private void SwitchToAlarmView()
        {
            this.ClientSize = new System.Drawing.Size(500, 325);

            // Configuration controls
            lblHoraActual.Visible = false;
            timePicker.Visible = false;
            btnGuardarAlarma.Visible = false;
            tableLayoutPanelOnOffButtons.Visible = false;

            // Alarm controls
            lblFrase.Visible = true;
            tableLayoutPanelAlarmButtons.Visible = true;

            lblFrase.Text = _phraseProvider.ObtenerFrase();
        }

        private async void ShowWindow()
        {
            await UpdateUiForCurrentState();
            this.Show();
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
            this.CenterToScreen();
        }

        private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (_hideOnClose)
                {
                    e.Cancel = true;
                    this.Hide();
                }
                else
                {
                    Environment.Exit(0);
                }
            }
        } 

        private void btnCerrar_Click(object? sender, EventArgs e)
        {
            _audioPlayer.StopAlarmSound();
            using (var dialog = new ConfirmationDialog(I18n.GetString("SleepWellMessage")))
            {
                dialog.ShowDialog();
            }
            Environment.Exit(0);
        }

        private void UpdateOnOffButtonStates(bool isAlarmActive)
        {
            if (btnBloquearAlarma != null)
            {
                btnBloquearAlarma.Enabled = true; 
            }

            if (btnDeactivateAlarm != null)
            {
                btnDeactivateAlarm.Enabled = isAlarmActive;
                btnDeactivateAlarm.Text = I18n.GetString("DeactivateAlarmButtonText") + I18n.GetString("DeactivateAlarmButtonIcon");
            }
        }

        private async void btnPosponer_Click(object? sender, EventArgs e)
        {
            _hideOnClose = true;
            _audioPlayer.StopAlarmSound();
            
            int postponeMinutes = _currentPostponeMinutes ?? 10;
            _alarmManager.Posponer(postponeMinutes);

            var settings = await _settingsManager.LoadSettingsAsync();
            settings.SnoozeUntil = DateTime.Now.AddMinutes(postponeMinutes);
            await _settingsManager.SaveSettingsAsync(settings);
            
            await UpdateUiForCurrentState();

            using (var dialog = new ConfirmationDialog(I18n.GetString("AlarmPostponedMessage", postponeMinutes)))
            {
                dialog.ShowDialog();
            }

            SwitchToConfigurationView();
        }

        private async Task ClearSnoozeStateAsync()
        {
            var settings = await _settingsManager.LoadSettingsAsync();
            if (settings.SnoozeUntil.HasValue)
            {
                settings.SnoozeUntil = null;
                await _settingsManager.SaveSettingsAsync(settings);
            }
        }

        private async Task UpdateUiForCurrentState()
        {
            var settings = await _settingsManager.LoadSettingsAsync();
            bool isSnoozing = settings.SnoozeUntil.HasValue && DateTime.Now < settings.SnoozeUntil.Value;

            if (isSnoozing)
            {
                // While snoozing, lock everything.
                timePicker.Enabled = false;
                btnGuardarAlarma.Enabled = false;
                btnDeactivateAlarm.Enabled = false; // Disable Deactivate button
                
                // Explicitly set btnBloquearAlarma to show "Unlock" text
                // as the UI is effectively locked by the snooze.
                if (btnBloquearAlarma != null)
                {
                    btnBloquearAlarma.Text = I18n.GetString("UnlockAlarmButtonText");
                    btnBloquearAlarma.Enabled = false; // Disable it
                }

                UpdateAlarmTimeLabel(settings.SnoozeUntil.Value); // Show snooze time
            }
            else
            {
                // Not snoozing, apply standard lock state
                btnGuardarAlarma.Enabled = true;
                btnBloquearAlarma.Enabled = true;
                ApplyLockState(settings.IsLocked);
                UpdateAlarmTimeLabel(settings.AlarmTime); // Show main alarm time
            }
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