using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace WinformsGUI
{
    public partial class Forside
    {
        private void CallPythonScript(string numbers)
        {
            try
            {
                // Minimize the form
                this.WindowState = FormWindowState.Minimized;

                // Path to your Python interpreter
                string pythonPath = @"C:\anaconda\python.exe";

                // Path to your Python script
                string scriptPath = @"C:\Users\Ali Najafi\OneDrive - Aarhus universitet\Dokumenter\pytautogui\test.py";

                // Create a new process to run the Python script
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = pythonPath,
                    Arguments = $"\"{scriptPath}\" {numbers}",  // Pass all numbers as a single argument
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(startInfo))
                {
                    // Read the output of the script
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    // Wait for the process to exit
                    process.WaitForExit();

                    // Restore and maximize the form after the Python script completes
                    this.Invoke((MethodInvoker)delegate
                    {
                        this.WindowState = FormWindowState.Maximized; // Restore and maximize the window
                        this.Activate(); // Bring the window to the foreground
                    });

                    // Display output or error if needed
                    if (!string.IsNullOrEmpty(output))
                    {
                        // Handle script output if necessary
                        Console.WriteLine($"Output: {output}"); // For debugging
                    }

                    if (!string.IsNullOrEmpty(error))
                    {
                        MessageBox.Show($"Error: {error}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }



        private void btnRunPython_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;


            // Calculate the sum of product prices and convert to decimal
            decimal totalPrice = scannedProducts.Sum(product => product.Price);

            // Convert the decimal total to an integer, removing any decimal part
            int totalInt = (int)totalPrice; // This will truncate the decimal part

            // Convert the integer total to string
            string numberString = totalInt.ToString();

            // Call Python script once with the full number string
            CallPythonScript(numberString);
        }
    }
}
