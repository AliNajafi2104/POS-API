using System;
using System.Linq;
using System.Media;
using System.Net.Http;
using System.Windows.Forms;
namespace WinformsGUI
{

    public partial class Forside
    {

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



    }
}
