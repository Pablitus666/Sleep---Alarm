namespace AlarmaSueño
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblFrase;
        private PressableImageButton btnCerrar;
        private PressableImageButton btnPosponer;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelAlarmButtons;
        private AlarmaSueño.PressableImageButton btnBloquearAlarma;
        private AlarmaSueño.PressableImageButton btnDeactivateAlarm;
        private AlarmaSueño.PressableImageButton btnGuardarAlarma;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelOnOffButtons;
        private AlarmaSueño.CustomDateTimePicker timePicker;
        private System.Windows.Forms.Label lblHoraActual;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.lblHoraActual = new System.Windows.Forms.Label();
            this.timePicker = new AlarmaSueño.CustomDateTimePicker();
            this.btnGuardarAlarma = new AlarmaSueño.PressableImageButton();
            this.tableLayoutPanelOnOffButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btnBloquearAlarma = new AlarmaSueño.PressableImageButton();
            this.btnDeactivateAlarm = new AlarmaSueño.PressableImageButton();
            this.lblFrase = new System.Windows.Forms.Label();
            this.tableLayoutPanelAlarmButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btnCerrar = new AlarmaSueño.PressableImageButton();
            this.btnPosponer = new AlarmaSueño.PressableImageButton();

            this.btnCerrar.ImageNormal = AlarmaSueño.ResourceLoader.LoadImage("boton.png");
            this.btnPosponer.ImageNormal = AlarmaSueño.ResourceLoader.LoadImage("boton.png");
            this.btnBloquearAlarma.ImageNormal = AlarmaSueño.ResourceLoader.LoadImage("boton.png");
            this.btnDeactivateAlarm.ImageNormal = AlarmaSueño.ResourceLoader.LoadImage("boton.png");
            this.btnGuardarAlarma.ImageNormal = AlarmaSueño.ResourceLoader.LoadImage("boton.png");

            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.tableLayoutPanelOnOffButtons.SuspendLayout();
            this.tableLayoutPanelAlarmButtons.SuspendLayout();
            this.SuspendLayout();

            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.pictureBoxLogo, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.lblHoraActual, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.timePicker, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.btnGuardarAlarma, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.tableLayoutPanelOnOffButtons, 0, 4);
            this.tableLayoutPanelMain.Controls.Add(this.lblFrase, 0, 5);
            this.tableLayoutPanelMain.Controls.Add(this.tableLayoutPanelAlarmButtons, 0, 6);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 7;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.TabIndex = 0;

            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBoxLogo.Location = new System.Drawing.Point(175, 5);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(150, 150);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxLogo.TabIndex = 0;
            this.pictureBoxLogo.TabStop = false;
            this.pictureBoxLogo.Click += new System.EventHandler(this.pictureBoxLogo_Click);

            //
            // lblHoraActual
            //
            this.lblHoraActual.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblHoraActual.AutoSize = true;
            this.lblHoraActual.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblHoraActual.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(214)))), ((int)(((byte)(226)))));
            this.lblHoraActual.Name = "lblHoraActual";
            this.lblHoraActual.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0); // Top padding
            this.lblHoraActual.Size = new System.Drawing.Size(129, 35);
            this.lblHoraActual.TabIndex = 1;
            this.lblHoraActual.Text = "Hora de Alarma";

            //
            // timePicker
            //
            this.timePicker.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.timePicker.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.timePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.timePicker.Location = new System.Drawing.Point(125, 203);
            this.timePicker.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10); // Top and bottom margin
            this.timePicker.Name = "timePicker";
            this.timePicker.ShowUpDown = true;
            this.timePicker.Size = new System.Drawing.Size(250, 36);
            this.timePicker.TabIndex = 2;

            //
            // btnGuardarAlarma
            //
            this.btnGuardarAlarma.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnGuardarAlarma.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnGuardarAlarma.ForeColor = System.Drawing.Color.White;
            this.btnGuardarAlarma.Location = new System.Drawing.Point(110, 259);
            this.btnGuardarAlarma.Name = "btnGuardarAlarma";
            this.btnGuardarAlarma.Size = new System.Drawing.Size(280, 60);
            this.btnGuardarAlarma.TabIndex = 3;
            this.btnGuardarAlarma.Click += new System.EventHandler(this.btnGuardarAlarma_Click);

            // 
            // tableLayoutPanelOnOffButtons
            // 
            this.tableLayoutPanelOnOffButtons.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanelOnOffButtons.ColumnCount = 2;
            this.tableLayoutPanelOnOffButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelOnOffButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelOnOffButtons.Controls.Add(this.btnBloquearAlarma, 0, 0);
            this.tableLayoutPanelOnOffButtons.Controls.Add(this.btnDeactivateAlarm, 1, 0);
            this.tableLayoutPanelOnOffButtons.Location = new System.Drawing.Point(40, 325);
            this.tableLayoutPanelOnOffButtons.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10); // Top and bottom margin
            this.tableLayoutPanelOnOffButtons.Name = "tableLayoutPanelOnOffButtons";
            this.tableLayoutPanelOnOffButtons.RowCount = 1;
            this.tableLayoutPanelOnOffButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelOnOffButtons.Size = new System.Drawing.Size(420, 60);
            this.tableLayoutPanelOnOffButtons.TabIndex = 4;

            //
            // btnBloquearAlarma
            //
            this.btnBloquearAlarma.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBloquearAlarma.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnBloquearAlarma.ForeColor = System.Drawing.Color.White;
            this.btnBloquearAlarma.Name = "btnBloquearAlarma";
            this.btnBloquearAlarma.Size = new System.Drawing.Size(180, 50);
            this.btnBloquearAlarma.TabIndex = 0;
            this.btnBloquearAlarma.Click += new System.EventHandler(this.btnBloquearAlarma_Click);

            //
            // btnDeactivateAlarm
            //
            this.btnDeactivateAlarm.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDeactivateAlarm.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnDeactivateAlarm.ForeColor = System.Drawing.Color.White;
            this.btnDeactivateAlarm.Name = "btnDeactivateAlarm";
            this.btnDeactivateAlarm.Size = new System.Drawing.Size(180, 50);
            this.btnDeactivateAlarm.TabIndex = 1;
            this.btnDeactivateAlarm.Click += new System.EventHandler(this.btnDeactivateAlarm_Click);

            // 
            // lblFrase
            // 
            this.lblFrase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFrase.AutoSize = true;
            this.lblFrase.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblFrase.ForeColor = System.Drawing.Color.White;
            this.lblFrase.Location = new System.Drawing.Point(0, 0);
            this.lblFrase.Name = "lblFrase";
            this.lblFrase.Size = new System.Drawing.Size(500, 25);
            this.lblFrase.TabIndex = 0;
            this.lblFrase.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblFrase.Visible = false;

            //
            // tableLayoutPanelAlarmButtons
            //
            this.tableLayoutPanelAlarmButtons.ColumnCount = 2;
            this.tableLayoutPanelAlarmButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelAlarmButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelAlarmButtons.Controls.Add(this.btnCerrar, 0, 0);
            this.tableLayoutPanelAlarmButtons.Controls.Add(this.btnPosponer, 1, 0);
            this.tableLayoutPanelAlarmButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelAlarmButtons.Location = new System.Drawing.Point(3, 401);
            this.tableLayoutPanelAlarmButtons.Margin = new System.Windows.Forms.Padding(3, 40, 3, 3);
            this.tableLayoutPanelAlarmButtons.Name = "tableLayoutPanelAlarmButtons";
            this.tableLayoutPanelAlarmButtons.RowCount = 1;
            this.tableLayoutPanelAlarmButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelAlarmButtons.Size = new System.Drawing.Size(500, 60);
            this.tableLayoutPanelAlarmButtons.Visible = false;

            //
            // btnCerrar
            //
            this.btnCerrar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCerrar.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnCerrar.ForeColor = System.Drawing.Color.White;
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(180, 50);
            this.btnCerrar.TabIndex = 0;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);

            //
            // btnPosponer
            //
            this.btnPosponer.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPosponer.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnPosponer.ForeColor = System.Drawing.Color.White;
            this.btnPosponer.Name = "btnPosponer";
            this.btnPosponer.Size = new System.Drawing.Size(180, 50);
            this.btnPosponer.TabIndex = 1;
            this.btnPosponer.Click += new System.EventHandler(this.btnPosponer_Click);

            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(2, 48, 71);
            this.ClientSize = new System.Drawing.Size(500, 450);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.tableLayoutPanelOnOffButtons.ResumeLayout(false);
            this.tableLayoutPanelAlarmButtons.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion
    }
}