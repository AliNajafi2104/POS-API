using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;



using System.Net.Http;

using FunctionLibrary.Models;
using Google.Protobuf.Reflection;
using searchengine123.Models;
using System.Linq;


namespace searchengine123
{
    public partial class Forside : Form
    {


        public Forside()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyPress += Form1_KeyPress;
            dataGridViewBasket.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dataGridViewBasket.MultiSelect = false;
            tbBarcode.Enabled = false;
            this.WindowState = FormWindowState.Maximized;
            this.Click += Button_Click;
            dataGridViewBasket.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dataGridViewBasket.MultiSelect = false;
            timer1.Start();
            // Assuming dgv is your DataGridView instance
            

            // Immediately update the labels with the current date and time
            UpdateDateTime();

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Update labels with current date and time on every tick
            UpdateDateTime();
        }


        SQL_Product SQL = new SQL_Product();
        TransactionService transactionService = new TransactionService();

        private async void btnAddToBasket_Click_1(object sender, EventArgs e)
        {
            if (scannedProducts.Count == 0)
            {
                timeStart = DateTime.Now;
            }
            if (tbBarcode.Text == "")
            {
                return;
            }
            try
            {
                Product produkt = await SQL.GetProductFromApiAsync(tbBarcode.Text);
                if (produkt == null)
                {
                    // Handle case where product is not found in the database
                    MessageBox.Show("Product not found in the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox1.Text = tbBarcode.Text;
                    tbBarcode.Clear();
                    return; // Exit method early
                }
                scannedProducts.Add(produkt);
                totalSum_CurrentBasket += Convert.ToDecimal(produkt.Price);
                dataGridViewBasketRefresh();
                tbBarcode.Clear();
            }
            catch (HttpRequestException ex)
            {
                // Handle case where API is unavailable
                MessageBox.Show("API is unavailable. Please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Handle any other unexpected exceptions
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            dataGridViewBasket.Columns[3].Visible = false;
        }


        public async void btnOrderConfirmed_Click(object sender, EventArgs e)
        {
            scannedProducts.Clear();
            dataGridViewBasketRefresh();


        }

        private void btnResetBasket_Click(object sender, EventArgs e)
        {
            clearBasket();
        }


        private void ManuelPrice_Click(object sender, EventArgs e)
        {
            dataGridViewBasket.RowTemplate.Height = 70;


            Control control = sender as Control;

            if (decimal.TryParse(tbManuelPrice.Text, out decimal manuelPrice))
            {
                scannedProducts.Add(new Product
                {
                    Name = control.Text,
                    Barcode = null,
                    Price = manuelPrice,

                });
                totalSum_CurrentBasket += manuelPrice;
                dataGridViewBasketRefresh();

                tbManuelPrice.Clear();
                btnAddToBasket.Focus();
            }


        }
        private void click(object sender, EventArgs e)
        {
            Control control = sender as Control;


            if (textBox2.Text != "")
            {
                textBox3.Text += control.Text;
            }
            else
            {

                if (control.Text == ",")
                {
                    tbManuelPrice.Text += ",";
                }
                else if (control.Text == "c")
                {
                    tbManuelPrice.Clear();
                }
                else if (control.Text == "")
                {
                    if (tbManuelPrice.Text.Length > 0)
                    {
                        tbManuelPrice.Text = tbManuelPrice.Text.Substring(0, tbManuelPrice.Text.Length - 1);
                    }
                }




                else if (multiply)
                {

                    int.TryParse(control.Text, out multiplier);
                    multiply = false;



                }
                else
                {

                    tbManuelPrice.Text += control.Text;
                    multiplier = 1;


                }

            }


        }




        DateTime timeStart;
        DateTime timeStop;

        public void dataGridViewBasketRefresh()
        {
            dataGridViewBasket.DataSource = null;
            dataGridViewBasket.DataSource = scannedProducts;
            dataGridViewBasket.ClearSelection();
            label2.Text = $"Total:   {totalSum_CurrentBasket:C}";


            dataGridViewBasket.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dataGridViewBasket.MultiSelect = false;


        }










        List<Product> scannedProducts = new List<Product>();
        decimal totalSum_CurrentBasket;
        bool multiply;
        int multiplier = 1;









        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9')
            {
                tbBarcode.AppendText(e.KeyChar.ToString());
            }
            else if (e.KeyChar == (char)Keys.Enter)
            {

                btnAddToBasket.PerformClick();
                e.Handled = true;
            }
            else if (e.KeyChar == '\b' && tbBarcode.Text.Length > 0)
            {
                tbBarcode.Text = tbBarcode.Text.Remove(tbBarcode.Text.Length - 1);


                tbBarcode.SelectionStart = tbBarcode.Text.Length;
                e.Handled = true;
            }
            else if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {

                e.Handled = true;
            }
        }


        private void clearBasket()
        {
            scannedProducts.Clear();
            totalSum_CurrentBasket = 0;
            dataGridViewBasketRefresh();
            btnAddToBasket.Focus();
        }








        private void button9_Click(object sender, EventArgs e)
        {
            multiply = true;
            this.ActiveControl = null;
        }



        private void button12_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            this.Close();
        }




        private void Button_Click(object sender, EventArgs e)
        {

            Button button = sender as Button;


            textBox2.Text += button.Text; // Append the text of the clicked button to textBox2
            btnAddToBasket.Focus();


        }

        private void Button_Click(object sender, KeyPressEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Product product = new Product
            {

                Barcode = textBox1.Text,
                Name = textBox2.Text,
                Price = Convert.ToDecimal(textBox3.Text),
                ProductTypeID = 1
            };


            try
            {
                SQL.CreateProductAsync(product);


                MessageBox.Show("Vare oprettet");
            }

            catch (Exception ex)
            {
                MessageBox.Show("error" + ex.Message);
            }

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            btnAddToBasket.Focus();
        }

        private void button38_Click(object sender, EventArgs e)
        {
            decimal amount = 0;
            foreach (var item in scannedProducts)
            {
                amount += item.Price;
            }
            TransactionDTO transaction = new TransactionDTO
            {
                ActionType = "Køb",
                StartTimestamp = timeStart,
                EndTimestamp = DateTime.Now,
                PaymentMethodID = 1,
                Amount = amount,
                SalespersonID = 1

            };
            try
            {

                transactionService.registerTransaction(transaction);
                MessageBox.Show("Transaktion registreret");
            }

            catch (Exception ex)
            {
                MessageBox.Show("error" + ex.Message);
            }
        }

        private void UpdateDateTime()
        {
            // Update the labels with the current date and time
            label5.Text = DateTime.Now.ToString("yyyy-MM-dd"); // Customize date format as needed
            label6.Text = DateTime.Now.ToString("HH:mm");   // Customize time format as needed
        }
        private void label5_Click(object sender, EventArgs e)
        {

        }

        private async void button40_Click(object sender, EventArgs e)
        {
            try
            {
                ReportService reportService = new ReportService();
                await reportService.GenerateZReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating Z Report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Optionally, log the exception or perform additional error handling as needed
            }
        }

        private void button41_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            btnAddToBasket.Focus();
        }
    }
}