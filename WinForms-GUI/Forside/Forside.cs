using Microsoft.AspNetCore.SignalR.Client;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace WinformsGUI
{
    public partial class Forside : Form
    {

        private readonly List<Product> scannedProducts = new List<Product>();
        private readonly ProductService productService = new ProductService(Environment.GetEnvironmentVariable("x-api-key"));
        private readonly List<PricePrKg> pricePrKgs = new List<PricePrKg>();
        private readonly List<StykPris> stykPris = new List<StykPris>();


        public Forside()
        {
            string filePath = "PricePrKgLibrary.json";
            string filepath2 = "StykPrisLibrary.json";
            // Read the JSON file content
            string json = File.ReadAllText(filePath);
            string json2 = File.ReadAllText(filepath2);
            // Deserialize the JSON string to a List<PricePrKg>
            pricePrKgs = JsonSerializer.Deserialize<List<PricePrKg>>(json);
            stykPris = JsonSerializer.Deserialize<List<StykPris>>(json2);
            InitializeComponent();
            InitializeFormSettings();
            InitializeSignalR();
            tabControl1.SelectedTab = tab2;
            setButtonImages();
        }


        private void setButtonImages()
        {
            cassava.Image = Image.FromFile("Images/cassava.jpg");
            sødKartoffel.Image = Image.FromFile("Images/sødKartoffel.jpg");
            koriander.Image = Image.FromFile("Images/koriander.png");
            dild.Image = Image.FromFile("Images/dild.jpg");
            mynte.Image = Image.FromFile("Images/mynte.png");
            persille.Image = Image.FromFile("Images/persille.jpeg");
            brød.Image = Image.FromFile("Images/brød.png");
            aflangPideBrød.Image = Image.FromFile("Images/aflangPideBrød.png"); 

        }



        private void InitializeFormSettings()
        {

            KeyPreview = true;
            KeyPress += Form1_KeyPress;
            WindowState = FormWindowState.Maximized;
            panel1.Visible = false;
            panelVareOprettet.Visible = false;
            tbBarcode.Enabled = false;
            timer1.Start();
            pictureBox1.Image = Properties.Resources.green_check;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dataGridView1.RowTemplate.Height = 50;
            dataGridView1.DataBindingComplete += DataGridView1_DataBindingComplete;


        }





        //FYSISK KEYBOARD
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
        //FYSISK KEYBOARD








        #region DATAGRIDVIEW
        private void DataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // Set column widths after data binding is complete
            if (dataGridView1.Columns.Count > 2) // Ensure there are enough columns
            {
                dataGridView1.Columns[1].Width = 138;
                dataGridView1.Columns[2].Width = 95;
                //dataGridView1.Columns[3].Width = 110;
            }
        }
        private void DeleteBasketProduct_CellClick(object sender, DataGridViewCellEventArgs e)
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






        #endregion

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    Button clickedButton = sender as Button;
        //    decimal price = Convert.ToDecimal((pricePrKgs.FirstOrDefault(n => n.Name == clickedButton.Name).pricePrKg)) / 1000;

        //    string result = Regex.Replace(clickedButton.Name, "(?<!^)([A-Z])", " $1").ToLower();

        //    scannedProducts.Add(new Product
        //    {
        //        Name = result + " " + tbManuelPrice.Text + "g",
        //        Price = Convert.ToDecimal(tbManuelPrice.Text) * price


        //    });
        //    UpdateDataGridView();

        //}

        private void button9_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tab2;
        }

        private void button38_Click(object sender, EventArgs e)
        {

        }

        private void dild_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;

            scannedProducts.Add(new Product
            {
                Name = clickedButton.Name,
                Price = Convert.ToDecimal(stykPris.FirstOrDefault(n => n.Name == clickedButton.Name).stykPris)
            
            });
            UpdateDataGridView();


        }

        private void VareOprettetOK_Click(object sender, EventArgs e)
        {
            panelVareOprettet.Visible = false;
            FocusButton();

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

                    default:

                        break;
                }
            }
            FocusButton();
        }
        private void btnClose_Click(object sender, EventArgs e) => Close();



        #region DATABASE
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
                panelVareOprettet.Visible = true;
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
        private void BtnDeleteProduct_Click(object sender, EventArgs e)
        {
            var sletVare = new SletVare();
            sletVare.ShowDialog();
            FocusButton();
        }
        #endregion



        #region BASKET RELATED

        private void BtnBetal_Click(object sender, EventArgs e)
        {
            decimal totalPrice = scannedProducts.Sum(p => p.Price);
            // Update the label with the total price
            label7.Text = $"Seneste kurv: {totalPrice:C}";

            btnResetBasket.PerformClick();
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
                    SystemSounds.Hand.Play();
                    tabControl1.SelectedTab = tabPage1;
                    ProductNotFoundPopUp();

                    return;
                }
                SystemSounds.Beep.Play();
                scannedProducts.Add(product);
                tbBarcode.Clear();


            }
            catch (HttpRequestException ex)
            {
                ShowError("API is unavailable. Please try again later.", ex);
                return;
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
        private void btnResetBasket_Click(object sender, EventArgs e)
        {
            scannedProducts.Clear();
            UpdateDataGridView();
            FocusButton();
        }
        #endregion




        #region NUMPAD & KEYBOARD

        #endregion



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
            //dataGridView1.Columns["ProductID"].Visible = false;
            dataGridView1.Refresh();

        }
        private void ClearTextBoxes(params TextBox[] textBoxes)
        {
            foreach (var textBox in textBoxes)
            {
                textBox.Clear();
            }
        }

        private void ProductNotFoundPopUp()
        {
            panel1.Visible = false;
            var popUp = new PopUp();
            popUp.ShowDialog();
            tbBarcodeCreate.Text = tbBarcode.Text;
            ClearTextBoxes(tbBarcode, tbPriceCreate, tbBarcode);
        }




        private HubConnection _hubConnection;
        private async void InitializeSignalR()
        {

            _hubConnection = new HubConnectionBuilder()
                .WithUrl($"http://{Config.IP_ADDRESS}:2030/notificationHub")
                .Build();


            _hubConnection.On<ProductResponse>("ReceiveProduct", result =>
            {



                Invoke(new Action(() =>
                {
                    if (result.Product != null)
                    {
                        SystemSounds.Beep.Play();
                        scannedProducts.Add(result.Product);
                        UpdateDataGridView();
                    }
                    else
                    {
                        SystemSounds.Hand.Play();
                        ProductNotFoundPopUp();
                        tbBarcodeCreate.Text = result.Barcode;
                    }


                }));
            });

            try
            {

                await _hubConnection.StartAsync();
                Console.WriteLine("SignalR connection started.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting SignalR connection: {ex.Message}");
            }
        }

    }

}




