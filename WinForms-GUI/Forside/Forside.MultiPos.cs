using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinformsGUI
{
    public partial class Forside
    {
        private void BetalButton_Click(object sender, EventArgs e)
        {
            decimal totalPrice = scannedProducts.Sum(p => p.Price);
            // Update the label with the total price
            label7.Text = $"Seneste kurv: {totalPrice:C}";

            btnResetBasket.PerformClick();
            /*

            // Window title of the Calculator application in Danish
            string windowTitle = "lommeregner";

            // Find the window by its title
            IntPtr hWnd = FindWindow(null, windowTitle);

            if (hWnd == IntPtr.Zero)
            {
                MessageBox.Show("Calculator window not found!");
                return;
            }

            // Restore the window if it is minimized
            this.WindowState = FormWindowState.Minimized;

            // Maximize the window
            ShowWindow(hWnd, SW_MAXIMIZE);

            // Bring the window to the foreground
            */
        }
        private void InitializeCustomComponents()
        {
            maximizeButton = new Button
            {
                Text = "Maximize Other Window",
                Dock = DockStyle.Fill
            };
            maximizeButton.Click += BtnBetal_Click;

            Controls.Add(maximizeButton);
        }

        // Import the FindWindow function
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        // Import the ShowWindow function
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        // Import the SetForegroundWindow function
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        // Import the IsIconic function to check if the window is minimized
        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);

        // Constants for ShowWindow
        private const int SW_RESTORE = 9;  // Restore a minimized window
        private const int SW_MAXIMIZE = 3; // Maximize the window
        private Button maximizeButton;
    }
}
