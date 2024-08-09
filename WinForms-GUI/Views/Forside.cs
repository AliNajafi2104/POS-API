using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;



using System.Net.Http;

using FunctionLibrary.Models;
using Google.Protobuf.Reflection;
using searchengine123.Models;
using System.Linq;
using searchengine123.Properties;
using searchengine123.Views;
using System.Threading.Tasks;


namespace searchengine123
{
    public partial class Forside : Form
    {
        private readonly List<Product> scannedProducts = new List<Product>();

        private readonly ProductService productService = new ProductService();

        public Forside()
        {
            InitializeComponent();
            InitializeFormSettings();
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
        private void ClearTextBoxes(params TextBox[] textBoxes)
        {
            foreach (var textBox in textBoxes)
            {
                textBox.Clear();
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
        private async void btnAddToBasket_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbBarcode.Text)) return;

            panel1.Visible = true;

            try
            {
                var product = await productService.GetProductFromApiAsync(tbBarcode.Text);
                if (product == null)
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

        private void ShowPopUp()
        {
            panel1.Visible = false;
            var popUp = new PopUp();
            popUp.ShowDialog();
            tbBarcodeCreate.Text = tbBarcode.Text;
            ClearTextBoxes(tbBarcode, tbPriceCreate, tbBarcode);
        }

        private void ShowError(string message, Exception ex)
        {
            MessageBox.Show(message, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void UpdateDataGridView()
        {

            displayTotal.Text = $"Total: {scannedProducts.Sum(product => product.Price):C}";
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = scannedProducts;
            dataGridView1.Columns["Id"].Visible = false;
            dataGridView1.Refresh();

        }

        private void btnResetBasket_Click(object sender, EventArgs e)
        {
            scannedProducts.Clear();
            UpdateDataGridView();
            FocusButton();
        }

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

        private void FocusButton()
        {
            btnAddToBasket.Focus();
        }



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


        private void btnClose_Click(object sender, EventArgs e) => Close();

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Await the asynchronous GetBasket method
                List<Product> newProducts = await productService.GetBasket();

                if (newProducts != null)
                {
                    foreach (var newProduct in newProducts)
                    {
                        // Check if the product is already in the scannedProducts list
                        var existingProduct = scannedProducts.FirstOrDefault(p => p.Id == newProduct.Id);

                        if (existingProduct != null)
                        {

                        }
                        else
                        {
                            // Add the new product to the list if it doesn't already exist
                            scannedProducts.Add(newProduct);
                        }
                    }

                    // Update the UI or perform other actions with the updated list
                    UpdateDataGridView();
                }
                else
                {
                    // Handle the case where GetBasket returns null, if needed
                    MessageBox.Show("No products found or an error occurred while fetching products.");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions if needed
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

       

        private async void button4_Click(object sender, EventArgs e)
        {
            bool result = await productService.ResetPhoneBasket();
            if (result)
            {
                MessageBox.Show("Nulstillet");
            }
            else
            {
                MessageBox.Show("Nulstilling fejlede");
            }

        }
    }


}

