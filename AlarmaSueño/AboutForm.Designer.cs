namespace AlarmaSueño
{
    partial class AboutForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.PictureBox pictureBoxRobot;
        private System.Windows.Forms.Label labelInfo;
        private AlarmaSueño.PressableImageButton btnCerrar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBoxRobot = new System.Windows.Forms.PictureBox();
            this.labelInfo = new System.Windows.Forms.Label();
            this.btnCerrar = new AlarmaSueño.PressableImageButton();
            this.btnCerrar.ImageNormal = AlarmaSueño.ResourceLoader.LoadImage("boton.png");
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRobot)).BeginInit();
            this.SuspendLayout();
            
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.pictureBoxRobot, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.labelInfo, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.btnCerrar, 1, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.Padding = new System.Windows.Forms.Padding(10);
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(370, 250);
            this.tableLayoutPanel.TabIndex = 0;
            
            // 
            // pictureBoxRobot
            // 
            this.pictureBoxRobot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxRobot.Location = new System.Drawing.Point(13, 13);
            this.pictureBoxRobot.Name = "pictureBoxRobot";
            this.tableLayoutPanel.SetRowSpan(this.pictureBoxRobot, 2);
            this.pictureBoxRobot.Size = new System.Drawing.Size(124, 224);
            this.pictureBoxRobot.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxRobot.TabIndex = 0;
            this.pictureBoxRobot.TabStop = false;
            
            // 
            // labelInfo
            // 
            this.labelInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelInfo.ForeColor = System.Drawing.Color.White;
            this.labelInfo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.labelInfo.Location = new System.Drawing.Point(160, 10);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(200, 170);
            this.labelInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // btnCerrar
            // 
            this.btnCerrar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCerrar.Location = new System.Drawing.Point(180, 185);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(125, 50);
            this.btnCerrar.TabIndex = 1;
            this.btnCerrar.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnCerrar.ForeColor = System.Drawing.Color.White;

            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.ClientSize = new System.Drawing.Size(370, 250);
            this.Controls.Add(this.tableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.tableLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRobot)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
