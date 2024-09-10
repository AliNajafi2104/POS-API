//using System;
//using System.Linq;
//using System.Windows.Forms;

//namespace WinformsGUI
//{
//    public partial class Forside
//    {

//        private void ShowError(string message, Exception ex)
//        {
//            MessageBox.Show(message, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
//        }
//        private void FocusButton()
//        {
//            btnAddToBasket.Focus();
//        }
//        private void UpdateDataGridView()
//        {

//            displayTotal.Text = $"Total: {scannedProducts.Sum(product => product.Price):C}";
//            dataGridView1.DataSource = null;
//            dataGridView1.DataSource = scannedProducts;
//            //dataGridView1.Columns["ProductID"].Visible = false;
//            dataGridView1.Refresh();

//        }
//        private void ClearTextBoxes(params TextBox[] textBoxes)
//        {
//            foreach (var textBox in textBoxes)
//            {
//                textBox.Clear();
//            }
//        }

//        private void ProductNotFoundPopUp()
//        {
//            panel1.Visible = false;
//            var popUp = new PopUp();
//            popUp.ShowDialog();
//            tbBarcodeCreate.Text = tbBarcode.Text;
//            ClearTextBoxes(tbBarcode, tbPriceCreate, tbBarcode);
//        }
//    }
//}
