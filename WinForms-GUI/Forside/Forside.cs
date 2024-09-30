using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
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
            panel2.Visible = false;

            // Hook up the DrawItem event to hide the tabs

        }



        private void setButtonImages()
        {
            cassava.Image = Image.FromFile("Images/cassava.jpg");
            sødKartoffel.Image = Image.FromFile("Images/sødKartoffel.jpg");
            koriander.Image = Image.FromFile("Images/koriander.png");
            dild.Image = Image.FromFile("Images/dild.jpg");
            mynte.Image = Image.FromFile("Images/mynte.png");
            persille.Image = Image.FromFile("Images/persille.png");
            aflangPideBrød.Image = Image.FromFile("Images/aflangPideBrød.png");
            brød.Image = Image.FromFile("Images/brød.png");
            sødKartoffelLilla.Image = Image.FromFile("Images/sødKartoffel.png");
            kartofflerINet.Image = Image.FromFile("Images/kartofflerINet.png");
            gurkemeje.Image = Image.FromFile("Images/gurkemeje.png");
            ingefær.Image = Image.FromFile("Images/ingefær.png");
            grønChili.Image = Image.FromFile("Images/grønChili.png");
            rødChili.Image = Image.FromFile("Images/rødChili.png");
            chiliHabenero.Image = Image.FromFile("Images/habanero.png");
            løgINet.Image = Image.FromFile("Images/løgINet.png");
            rødLøg.Image = Image.FromFile("Images/rødLøg.png");
            okra.Image = Image.FromFile("Images/okra.png");
            miniAgurk.Image = Image.FromFile("Images/agurk.png");
            madbananer.Image = Image.FromFile("Images/madbananer.png");
            plantain.Image = Image.FromFile("Images/plantain.png");
            hvidAubergine.Image = Image.FromFile("Images/hvidAubergine.png");
            aubergine.Image = Image.FromFile("Images/aubergine.png");
            mango.Image = Image.FromFile("Images/mango.png");
            granatæbler.Image = Image.FromFile("Images/granatæble.png");
            vandmelon.Image = Image.FromFile("Images/vandmelon.png");
            lime.Image = Image.FromFile("Images/lime.png");
            citron.Image = Image.FromFile("Images/citron.png");
            abrikoser.Image = Image.FromFile("Images/abrikoser.png");
            valnødder.Image = Image.FromFile("Images/valnødder.png");

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

            string formattedName = FormatProductName(clickedButton.Name);

            scannedProducts.Add(new Product
            {
                Name = formattedName,
                Price = Convert.ToDecimal(stykPris.FirstOrDefault(n => n.Name == clickedButton.Name).stykPris)

            });
            UpdateDataGridView();
            this.ActiveControl = null;
            btnAddToBasket.Focus();


        }

        private string FormatProductName(string name)
        {
            // Use regular expression to insert spaces before each uppercase letter except the first one
            string formattedName = System.Text.RegularExpressions.Regex.Replace(name, "(?<!^)([A-Z])", " $1");

            // Capitalize the first letter of each word
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(formattedName.ToLower());
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

        private void cassava_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;

          

            if (clickedButton.Name == "indtastBeløb")
            {
                pictureBox2.Image = null;
                panel2.Visible = true;
                name = clickedButton.Name;
                label32.Text = "Indtast manuel pris";

            }
            else
            {
                if (clickedButton.Image == null) return;
                pictureBox2.Image = clickedButton.Image;
                panel2.Visible = true;
                name = clickedButton.Name;
                label32.Text = name + " " + (pricePrKgs.FirstOrDefault(n => n.Name == clickedButton.Name).pricePrKg) + " pr/KG";
                d = pricePrKgs.FirstOrDefault(n => n.Name == clickedButton.Name).pricePrKg;
            }

        }
        string name;
        private void button4_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            textBox1.Text += button.Text;
        }
        string d;
        private void button43_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                panel2.Visible = false;

            }
            else
            {
                scannedProducts.Add(new Product
                {

                    Name = name + " " + textBox1.Text + " g",
                    Price =  Convert.ToDecimal(d) * Convert.ToDecimal(textBox1.Text) / 1000

                });
                panel2.Visible = false;
                textBox1.Clear();
                UpdateDataGridView();
            }
            this.ActiveControl = null;
            btnAddToBasket.Focus();
        }

        private void Q_Click(object sender, EventArgs e)
        {

            Button button = sender as Button;

            if (button.Name == "space") tbNameCreate.Text += " ";
            else if (button.Text == "<--")
            {

                // Delete the last character from the text
                if (!string.IsNullOrEmpty(tbNameCreate.Text))
                {
                    tbNameCreate.Text = tbNameCreate.Text.Substring(0, tbNameCreate.Text.Length - 1);
                }

            }//add code here to delete last char in the text
            else tbNameCreate.Text += button.Text;
            this.ActiveControl = null;
            btnAddToBasket.Focus();
        }

        private void button50_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            tbPriceCreate.Text += button.Text;
            this.ActiveControl = null;
            btnAddToBasket.Focus();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            // Check the currently selected tab
            if (tabControl1.SelectedTab == tab2)
            {
                // If tab2 is selected, switch to the first tab
                tabControl1.SelectTab(0); // Assuming the first tab is at index 0
            }
            else
            {
                // If the first tab is selected, switch to tab2
                tabControl1.SelectTab(tab2);
            }

            // Clear the active control and set focus to btnAddToBasket
            this.ActiveControl = null;
            btnAddToBasket.Focus();
        }

        private void tbBarcodeDel_Click(object sender, EventArgs e)
        {
            tbBarcode.Text = "";
            this.ActiveControl = null;
            btnAddToBasket.Focus();
        }

        private void tbNameDel_Click(object sender, EventArgs e)
        {
            tbNameCreate.Text = "";
            this.ActiveControl = null;
            btnAddToBasket.Focus();
        }

        private void tbPriceDel_Click(object sender, EventArgs e)
        {
            tbPriceCreate.Text = "";
            this.ActiveControl = null;
            btnAddToBasket.Focus();
        }

        private void label32_Click(object sender, EventArgs e)
        {

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
                    case "tbManuelPriceDel":

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
            this.ActiveControl = null;
            btnAddToBasket.Focus();
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

        #endregion

        private void button42_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            this.ActiveControl = null;
            btnAddToBasket.Focus();
        }

        private void button41_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            this.ActiveControl = null;
            btnAddToBasket.Focus();

        }
    }


}

