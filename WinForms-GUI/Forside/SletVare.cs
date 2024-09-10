using System;
using System.Net.Http;
using System.Windows.Forms;

namespace WinformsGUI
{
    public partial class SletVare : Form
    {
        ProductService productService = new ProductService(Environment.GetEnvironmentVariable("x-api-key"));
        public SletVare()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate the input
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show("Please enter a valid product ID.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Call the service to delete the product
                await productService.DeleteProduct(textBox1.Text);

                // Notify the user of success
                MessageBox.Show("Product deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Close the form
                this.Close();
            }
            catch (HttpRequestException httpEx)
            {
                // Handle specific HTTP request errors
                MessageBox.Show($"Error communicating with the server: {httpEx.Message}", "Network Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Handle general errors
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



    }
}
