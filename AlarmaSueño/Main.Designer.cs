namespace AlarmaSueño
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblFrase;
        private PressableImageButton btnCerrar;
        private PressableImageButton btnPosponer;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private System.Windows.Forms.CheckBox chkAutoStart;
        private System.Windows.Forms.CheckBox chkLockSettings;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelAlarmButtons;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.chkAutoStart = new System.Windows.Forms.CheckBox();
            this.chkLockSettings = new System.Windows.Forms.CheckBox();
            this.lblFrase = new System.Windows.Forms.Label();
            this.tableLayoutPanelAlarmButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btnCerrar = new AlarmaSueño.PressableImageButton();
            this.btnPosponer = new AlarmaSueño.PressableImageButton();
            this.btnCerrar.ImageNormal = AlarmaSueño.ResourceLoader.LoadImage("boton.png");
            this.btnPosponer.ImageNormal = AlarmaSueño.ResourceLoader.LoadImage("boton.png");

            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.tableLayoutPanelAlarmButtons.SuspendLayout();
            this.SuspendLayout();
            
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.pictureBoxLogo, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.chkAutoStart, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.chkLockSettings, 0, 3);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 4;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
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
            // chkAutoStart
            // 
            this.chkAutoStart.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkAutoStart.AutoSize = true;
            this.chkAutoStart.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.chkAutoStart.ForeColor = System.Drawing.Color.White;
            this.chkAutoStart.Name = "chkAutoStart";
            this.chkAutoStart.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.chkAutoStart.Size = new System.Drawing.Size(193, 24);
            this.chkAutoStart.TabIndex = 2;
            this.chkAutoStart.UseVisualStyleBackColor = true;
            
            // 
            // chkLockSettings
            // 
            this.chkLockSettings.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkLockSettings.AutoSize = true;
            this.chkLockSettings.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.chkLockSettings.ForeColor = System.Drawing.Color.White;
            this.chkLockSettings.Name = "chkLockSettings";
            this.chkLockSettings.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.chkLockSettings.Size = new System.Drawing.Size(262, 29);
            this.chkLockSettings.TabIndex = 3;
            this.chkLockSettings.UseVisualStyleBackColor = true;
            
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
            
            // 
            // tableLayoutPanelAlarmButtons
            // 
            this.tableLayoutPanelAlarmButtons.ColumnCount = 2;
            this.tableLayoutPanelAlarmButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelAlarmButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelAlarmButtons.Controls.Add(this.btnCerrar, 0, 0);
            this.tableLayoutPanelAlarmButtons.Controls.Add(this.btnPosponer, 1, 0);
            this.tableLayoutPanelAlarmButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelAlarmButtons.Name = "tableLayoutPanelAlarmButtons";
            this.tableLayoutPanelAlarmButtons.RowCount = 1;
            this.tableLayoutPanelAlarmButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelAlarmButtons.Size = new System.Drawing.Size(500, 60);
            
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
            this.ClientSize = new System.Drawing.Size(500, 350);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.tableLayoutPanelAlarmButtons.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}