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


namespace searchengine123
{
    public partial class Forside : Form
    {
        public Forside()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyPress += Form1_KeyPress;
            this.WindowState = FormWindowState.Maximized;
            this.Click += Numpad;
            
            dataGridViewBasket.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dataGridViewBasket.DataSource = scannedProducts;
            dataGridViewBasket.Columns["ProductTypeID"].Visible = false;
            
            tbBarcode.Enabled = false;
            
            timer1.Start();
           
            InitiateHTTP();


        }

        private async void InitiateHTTP()
        {
            try
            { Product produkt = await productService.GetProductFromApiAsync("1");
            }

            catch (Exception ex)
            {
                MessageBox.Show("intital http error ", ex.Message);
            }
            }

       
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
                Product produkt = await productService.GetProductFromApiAsync(tbBarcode.Text);
                if (produkt == null)
                {
                    
                    MessageBox.Show("Product not found in the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tbBarcodeCreate.Text = tbBarcode.Text;
                    tbNameCreate.Text = "";
                    tbPriceCreate.Text = "";
                    tbBarcode.Clear();
                    return; 
                }
                scannedProducts.Add(produkt);
                totalSum_CurrentBasket += Convert.ToDecimal(produkt.Price);
                dataGridViewBasketRefresh();
                tbBarcode.Clear();
            }
            catch (HttpRequestException ex)
            {
                
                MessageBox.Show("API is unavailable. Please try again later.", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            dataGridViewBasket.Columns[3].Visible = false;
        }

        public void btnOrderConfirmed_Click(object sender, EventArgs e)
        {
            scannedProducts.Clear();
            dataGridViewBasketRefresh();


        }

        private void btnResetBasket_Click(object sender, EventArgs e)
        {
            scannedProducts.Clear();
            totalSum_CurrentBasket = 0;
            dataGridViewBasketRefresh();
            btnAddToBasket.Focus();
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
        private void Numpad(object sender, EventArgs e)
        {
            Control control = sender as Control;


            if (tbNameCreate.Text != "")
            {
                tbPriceCreate.Text += control.Text;
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
                else
                {
                    tbManuelPrice.Text += control.Text;
                }



                

            }
            btnAddToBasket.Focus();

        }
       
        public void dataGridViewBasketRefresh()
        {
            dataGridViewBasket.DataSource = null;
            dataGridViewBasket.DataSource = scannedProducts;
            dataGridViewBasket.ClearSelection();
            displayTotal.Text = $"Total:   {totalSum_CurrentBasket:C}";


            dataGridViewBasket.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dataGridViewBasket.MultiSelect = false;

            dataGridViewBasket.Columns["ProductTypeID"].Visible = false;
            dataGridViewBasket.Columns["ProductTypeID"].Visible = false;

            dataGridViewBasket.Columns[0].Width = 138;

            dataGridViewBasket.Columns[1].Width = 95;

            dataGridViewBasket.Columns[2].Width = 110;

        }

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

        private void Minimize(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.ActiveControl = null;
            btnAddToBasket.Focus();
        }

        private void Close(object sender, EventArgs e)
        {
            this.Close();
        }

        
        private void Keyboard(object sender, EventArgs e)
        {

            Button button = sender as Button;

            if(tbBarcodeCreate.Text!="")
            {
                if (button != null && button.Text != "-" && button.Text!="<--")
                {

                    tbNameCreate.Text += button.Text;
                }
                else if (button.Text == "-" && button != null)
                {


                    tbNameCreate.Text += " ";

                }
                else if (button.Text == "<--" && button != null)
                {
                    if (tbNameCreate.Text.Length > 0) // Ensure there is text to remove
                    {
                        tbNameCreate.Text = tbNameCreate.Text.Substring(0, tbNameCreate.Text.Length - 1);
                    }
                }

            }
            btnAddToBasket.Focus();
        }
        

        private async void CreateProduct(object sender, EventArgs e)
        {

            if (tbPriceCreate.Text != "")
            {

                Product product = new Product
                {

                    Barcode = tbBarcodeCreate.Text,
                    Name = tbNameCreate.Text,
                    Price = Convert.ToDecimal(tbPriceCreate.Text),
                    ProductTypeID = 1
                };


                try
                {
                   await productService.CreateProductAsync(product);


                    MessageBox.Show("Vare oprettet");
                    scannedProducts.Add(product);
                    totalSum_CurrentBasket += Convert.ToDecimal(product.Price);
                    dataGridViewBasketRefresh();
                }

                catch (Exception ex)
                {
                    MessageBox.Show("error" + ex.Message);
                }

                tbBarcodeCreate.Clear();
                tbNameCreate.Clear();
                tbPriceCreate.Clear();
                btnAddToBasket.Focus();
            }
            else
                tbBarcodeCreate.Clear();
            tbNameCreate.Clear();
            tbPriceCreate.Clear();
            btnAddToBasket.Focus();
            }

        private async void PaymentCash(object sender, EventArgs e)
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

               await transactionService.registerTransaction(transaction);
                MessageBox.Show("Transaktion registreret");
            }

            catch (Exception ex)
            {
                MessageBox.Show("error" + ex.Message);
            }
        }

       
       
        private async void GenerateZReport(object sender, EventArgs e)
        {
            try
            {
                ReportService reportService = new ReportService();
                await reportService.GenerateZReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating Z Report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
               
            }
        }

        
        DateTime timeStart;
        readonly List<Product> scannedProducts = new List<Product>();
        decimal totalSum_CurrentBasket;
        ProductService productService = new ProductService();
        readonly TransactionService transactionService = new TransactionService();

        private void button1_Click(object sender, EventArgs e)
        {
            tbPriceCreate.Text = "";
            this.ActiveControl = null;
            btnAddToBasket.Focus();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tbBarcode.Clear();
            this.ActiveControl = null;
            btnAddToBasket.Focus();
        }

        private void Forside_Load(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {
            tbNameCreate.Clear();
            this.ActiveControl = null;
            btnAddToBasket.Focus();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            tbManuelPrice.Clear();
            this.ActiveControl = null;
            btnAddToBasket.Focus();
        }
    }
}