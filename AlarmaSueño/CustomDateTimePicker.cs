using System;
using System.Windows.Forms;

namespace AlarmaSueño
{
    public class CustomDateTimePicker : DateTimePicker
    {
        private int digitCount = 0;

        public CustomDateTimePicker()
        {
            Format = DateTimePickerFormat.Custom;
            CustomFormat = "HH:mm";
            ShowUpDown = true;
            TabStop = true;

            KeyPress += OnKeyPressAdvanceSegment;
        }

        private void OnKeyPressAdvanceSegment(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar))
                return;

            digitCount++;

            // Cada segmento HH o mm tiene 2 dígitos
            if (digitCount == 2)
            {
                digitCount = 0;

                // Avanzar al siguiente segmento (HH → mm)
                BeginInvoke(new Action(() =>
                {
                    this.Focus();
                    SendKeys.Send("{RIGHT}");
                }));
            }
        }

        // The OnLostFocus override has been removed to allow natural focus flow.


        protected override bool ShowFocusCues => false;
    }
}