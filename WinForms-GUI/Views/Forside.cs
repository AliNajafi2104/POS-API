
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Windows.Forms;
using WinformsGUI.Models;


namespace WinformsGUI
{
    public partial class Forside : Form
    {

        #region VARIABLES
        private readonly List<Product> scannedProducts = new List<Product>();
        private readonly ProductService productService = new ProductService();
        private HubConnection _hubConnection;
        #endregion



        public Forside()
        {
            InitializeComponent();
            InitializeFormSettings();
            InitializeSignalR();

        }



        #region INITIALIZATION
        private async void InitializeSignalR()
        {
            // Initialize the connection to the SignalR hub
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:2030/notificationHub") // Use the correct URL for your SignalR hub
                .Build();

            // Define how to handle incoming messages
            _hubConnection.On<ProductResponse>("ReceiveProduct", result =>
            {

              
                // Update the UI on the main thread
                Invoke(new Action(() =>
                {
                   if(result.Product!=null)
                    {
                        scannedProducts.Add(result.Product);
                        UpdateDataGridView();
                    }
                   else
                    {
                        ShowPopUp();
                        tbBarcodeCreate.Text = result.Barcode;
                    }


                }));
            });

            try
            {
                // Start the connection
                await _hubConnection.StartAsync();
                Console.WriteLine("SignalR connection started.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting SignalR connection: {ex.Message}");
            }
        }

        private void InitializeFormSettings()
        {

            KeyPreview = true;
            KeyPress += Form1_KeyPress;
            WindowState = FormWindowState.Maximized;
            Click += HandleInput;
            panel1.Visible = false;
            panel2.Visible = false;
            tbBarcode.Enabled = false;
            timer1.Start();
            pictureBox1.Image = Properties.Resources.green_check;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dataGridView1.RowTemplate.Height = 50;
            dataGridView1.DataBindingComplete += DataGridView1_DataBindingComplete;


        }
        #endregion

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
                    this.Invoke((MethodInvoker)delegate {
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




        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Handle key press event
            if (e.KeyChar >= '0' && e.KeyChar <= '9')
            {
                tbBarcode.AppendText(e.KeyChar.ToString());
            }
            else if (e.KeyChar == '\b' && tbBarcode.Text.Length > 0)
            {
                tbBarcode.Text = tbBarcode.Text.Remove(tbBarcode.Text.Length - 1);
                tbBarcode.SelectionStart = tbBarcode.Text.Length;
                e.Handled = true; // Prevent further processing of this key
            }
            else if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Prevent invalid keys from being processed
            }
        }
        private void DataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // Set column widths after data binding is complete
            if (dataGridView1.Columns.Count > 2) // Ensure there are enough columns
            {
                dataGridView1.Columns[1].Width = 138;
                dataGridView1.Columns[2].Width = 95;
                dataGridView1.Columns[3].Width = 110;
            }
        }
        private void ShowPopUp()
        {
            panel1.Visible = false;
            var popUp = new PopUp();
            popUp.ShowDialog();
            tbBarcodeCreate.Text = tbBarcode.Text;
            ClearTextBoxes(tbBarcode, tbPriceCreate, tbBarcode);
        }
        private void HandleInput(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                // Determine if the button is a numpad or keyboard button based on its text
                if (char.IsDigit(button.Text[0]) || button.Text == ",")  // Check if the button's text is a digit
                {
                    HandleNumpadInput(button);
                }
                else
                {
                    HandleKeyboardInput(button);
                }
            }
            FocusButton();
        }
        private void HandleNumpadInput(Button button)
        {
            // Handle inputs for the numpad
            if (!string.IsNullOrWhiteSpace(tbNameCreate.Text))
            {
                tbPriceCreate.Text += button.Text;
            }
            else
            {
                switch (button.Text)
                {
                    case ",":
                        tbManuelPrice.Text += ",";
                        break;
                    case "c":
                        tbManuelPrice.Clear();
                        break;
                    case "":
                        if (tbManuelPrice.Text.Length > 0)
                            tbManuelPrice.Text = tbManuelPrice.Text.Substring(0, tbManuelPrice.Text.Length - 1);
                        break;
                    default:
                        tbManuelPrice.Text += button.Text;
                        break;
                }
            }
        }
        private void HandleKeyboardInput(Button button)
        {
            // Handle inputs for the keyboard
            if (!string.IsNullOrWhiteSpace(tbBarcodeCreate.Text))
            {
                if (button.Text == "-" || button.Text == "<--")
                {
                    if (button.Text == "-")
                    {
                        tbNameCreate.Text += " ";
                    }
                    else if (button.Text == "<--")
                    {
                        if (tbNameCreate.Text.Length > 0)
                            tbNameCreate.Text = tbNameCreate.Text.Substring(0, tbNameCreate.Text.Length - 1);
                    }
                }
                else
                {
                    tbNameCreate.Text += button.Text;
                }
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                var productToRemove = (Product)dataGridView1.Rows[e.RowIndex].DataBoundItem;
                if (productToRemove != null &&
                    MessageBox.Show(
                        $"Are you sure you want to delete {productToRemove.Name} priced at {productToRemove.Price:C}?",
                        "Delete Confirmation",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    ) == DialogResult.Yes)
                {
                    scannedProducts.Remove(productToRemove);
                    UpdateDataGridView();

                }
            }
            FocusButton();
        }




        #region HELPER FUNCTIONS
        private void ManuelPrice_Click(object sender, EventArgs e)
        {
            if (decimal.TryParse(tbManuelPrice.Text, out var manuelPrice))
            {
                scannedProducts.Add(new Product
                {
                    Name = (sender as Control)?.Text,
                    Barcode = null,
                    Price = manuelPrice,
                });
                tbManuelPrice.Clear();
                UpdateDataGridView();
                FocusButton();
            }
        }
        private void ShowError(string message, Exception ex)
        {
            MessageBox.Show(message, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void FocusButton()
        {
            btnAddToBasket.Focus();
        }
        private void UpdateDataGridView()
        {

            displayTotal.Text = $"Total: {scannedProducts.Sum(product => product.Price):C}";
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = scannedProducts;
            dataGridView1.Columns["Id"].Visible = false;
            dataGridView1.Refresh();

        }
        private void ClearTextBoxes(params TextBox[] textBoxes)
        {
            foreach (var textBox in textBoxes)
            {
                textBox.Clear();
            }
        }
        #endregion



        #region BUTTONS
        private void ClearTextBoxBasedOnButton(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                switch (button.Name)
                {
                    case "tbPriceDel":
                        tbPriceCreate.Clear();
                        break;
                    case "tbNameDel":
                        tbNameCreate.Clear();
                        break;
                    case "tbBarcodeDel":
                        tbBarcode.Clear();
                        break;
                    case "tbManuelPriceDel":
                        tbManuelPrice.Clear();
                        break;
                    default:

                        break;
                }
            }
            FocusButton();
        }
        private async void btnAddToBasket_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbBarcode.Text)) return;

            panel1.Visible = true;

            try
            {
                var product = await productService.GetProductFromApiAsync(tbBarcode.Text);
               if(product==null)
                {
                    ShowPopUp();
                    return;
                }
                scannedProducts.Add(product);
                tbBarcode.Clear();


            }
            catch (HttpRequestException ex)
            {
                ShowError("API is unavailable. Please try again later.", ex);
            }
            catch (Exception ex)
            {
                ShowError("An error occurred: " + ex.Message, ex);
            }
            finally
            {
                panel1.Visible = false;
                UpdateDataGridView();
            }
        }
        private async void CreateProduct(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbPriceCreate.Text)) return;

            var product = new Product
            {
                Barcode = tbBarcodeCreate.Text,
                Name = tbNameCreate.Text,
                Price = Convert.ToDecimal(tbPriceCreate.Text),
            };

            try
            {
                await productService.CreateProductAsync(product);
                panel2.Visible = true;
                scannedProducts.Add(product);
                ClearTextBoxes(tbBarcodeCreate, tbNameCreate, tbPriceCreate);
            }
            catch (Exception ex)
            {
                ShowError("error" + ex.Message, ex);
            }
            FocusButton();
            UpdateDataGridView();
        }
        private void btnResetBasket_Click(object sender, EventArgs e)
        {
            scannedProducts.Clear();
            UpdateDataGridView();
            FocusButton();
        }
        private void button9_Click(object sender, EventArgs e)
        {
            var sletVare = new SletVare();
            sletVare.ShowDialog();
            FocusButton();
        }
        private void button28_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            FocusButton();
        }
        private void btnClose_Click(object sender, EventArgs e) => Close();
        #endregion



    }


}

