using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
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
        private readonly List<PricePrKg> stykPris = new List<stykPris>();

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

      

        private void button9_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tab2;
        }

       
    }


}

