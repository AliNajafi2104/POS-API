using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinformsGUI
{
    public partial class Forside
    {
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
    }
}
