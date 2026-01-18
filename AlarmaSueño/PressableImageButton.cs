using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AlarmaSueño
{
    public class PressableImageButton : Control, IButtonControl
    {
        // --- State Management ---
        private enum ButtonVisualState { Normal, Hover, Pressed, Disabled }
        private ButtonVisualState _visualState = ButtonVisualState.Normal;
        private bool _isDefault = false; // Para IButtonControl

        // --- IButtonControl Properties ---
        [Category("Behavior")]
        [Description("El DialogResult que se devolverá al formulario padre cuando se haga clic en el botón.")]
        [DefaultValue(DialogResult.None)]
        public DialogResult DialogResult { get; set; } = DialogResult.None;

        // --- Custom Properties ---
        private Image? _imageNormal;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Image? ImageNormal { get => _imageNormal; set { _imageNormal = value; Invalidate(); } }

        private Image? _imageHover;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Image? ImageHover { get => _imageHover; set { _imageHover = value; Invalidate(); } }

        private Image? _imagePressed;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Image? ImagePressed { get => _imagePressed; set { _imagePressed = value; Invalidate(); } }

        private Image? _imageDisabled;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Image? ImageDisabled { get => _imageDisabled; set { _imageDisabled = value; Invalidate(); } }

        [Category("Behavior")]
        [Description("Desplazamiento en píxeles al presionar.")]
        [DefaultValue(2)]
        public int PressOffset { get; set; } = 2;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color TextColorNormal { get; set; } = Color.White;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color TextColorHover { get; set; } = ColorTranslator.FromHtml("#FFD700");

        public PressableImageButton()
        {
            this.SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor |
                ControlStyles.Selectable, // Permite que el control reciba foco
                true);
            
            this.BackColor = Color.Transparent;
            this.Size = new Size(150, 50); // Proporcionar un tamaño por defecto
        }

        // --- IButtonControl Methods ---
        public void NotifyDefault(bool value)
        {
            if (_isDefault != value)
            {
                _isDefault = value;
                this.Invalidate(); // Redibujar para mostrar/ocultar el borde de foco
            }
        }

        public void PerformClick()
        {
            if (this.CanSelect)
            {
                this.OnClick(EventArgs.Empty);
            }
        }


        // --- State Update Logic ---
        private void SetVisualState(ButtonVisualState newState)
        {
            if (_visualState != newState)
            {
                _visualState = newState;
                Invalidate();
            }
        }

        // --- Event Overrides for Mouse ---
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            SetVisualState(ButtonVisualState.Hover);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            SetVisualState(ButtonVisualState.Normal);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                this.Focus();
                SetVisualState(ButtonVisualState.Pressed);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Left)
            {
                if (this.ClientRectangle.Contains(e.Location))
                {
                    SetVisualState(ButtonVisualState.Hover);
                }
                else
                {
                    SetVisualState(ButtonVisualState.Normal);
                }
                // La clase base Control invoca OnClick en OnMouseUp,
                // así que no necesitamos llamarlo explícitamente aquí.
            }
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            // Si este botón está en un formulario de diálogo, cierra el diálogo.
            Form? parentForm = this.FindForm();
            if (parentForm != null && this.DialogResult != DialogResult.None)
            {
                parentForm.DialogResult = this.DialogResult;
            }
        }

        // --- Event Overrides for Keyboard and Focus ---
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            Invalidate(); 
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            SetVisualState(ButtonVisualState.Normal); 
            Invalidate(); 
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter)
            {
                e.Handled = true; // Indicar que hemos manejado la tecla
                SetVisualState(ButtonVisualState.Pressed);
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter)
            {
                e.Handled = true; // Indicar que hemos manejado la tecla
                if (_visualState == ButtonVisualState.Pressed)
                {
                    PerformClick(); // Usar el método de IButtonControl
                    SetVisualState(ButtonVisualState.Normal);
                }
            }
        }
        
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            SetVisualState(this.Enabled ? ButtonVisualState.Normal : ButtonVisualState.Disabled);
        }

        // --- Custom Painting ---
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            if (this.Parent != null)
            {
                g.Clear(this.Parent.BackColor);
            }

            var currentState = this.Enabled ? _visualState : ButtonVisualState.Disabled;
            Image? imageToDraw = GetCurrentImage(currentState);
            
            Rectangle contentRect = new Rectangle(0, 0, this.ClientSize.Width, this.ClientSize.Height);
            
            if (currentState == ButtonVisualState.Pressed)
            {
                contentRect.Offset(PressOffset, PressOffset);
            }
            
            if (imageToDraw != null)
            {
                g.DrawImage(imageToDraw, contentRect);
            }

            if (!string.IsNullOrEmpty(this.Text))
            {
                Color textColor;
                if (!this.Enabled)
                {
                    textColor = Color.Gray;
                }
                else if (currentState == ButtonVisualState.Hover && TextColorHover != Color.Transparent)
                {
                    textColor = this.TextColorHover;
                }
                else
                {
                    textColor = this.TextColorNormal;
                }
                
                Rectangle textRect = this.ClientRectangle;

                TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak;
                TextRenderer.DrawText(g, this.Text, this.Font, textRect, textColor, flags);
            }
        }

        private Image? GetCurrentImage(ButtonVisualState state)
        {
            switch (state)
            {
                case ButtonVisualState.Hover: return ImageHover ?? ImageNormal;
                case ButtonVisualState.Pressed: return ImagePressed ?? ImageHover ?? ImageNormal;
                case ButtonVisualState.Disabled: return ImageDisabled ?? ImageNormal;
                default: return ImageNormal;
            }
        }
    }
}