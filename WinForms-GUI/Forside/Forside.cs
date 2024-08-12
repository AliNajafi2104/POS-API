
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using PdfiumViewer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Windows.Forms;
using WinformsGUI.Models;
using System.IO;
using System.Runtime.InteropServices;


namespace WinformsGUI
{
    public partial class Forside : Form
    {
        
        private readonly List<Product> scannedProducts = new List<Product>();
        private readonly ProductService productService = new ProductService();
     
       
        public Forside()
        {
            InitializeComponent();
            InitializeFormSettings();
            InitializeSignalR();
            panelPdfViewer.Visible = false;
            pdf = new PdfiumViewer.PdfViewer();
            pdf.Dock = DockStyle.Fill; // Make the PdfViewer fill the panel
            panelPdfViewer.Controls.Add(pdf); // Add PdfViewer to the Panel
            maximizeButton = new Button
            {
                Text = "Maximize Other Window",
                Dock = DockStyle.Fill
            };
            maximizeButton.Click += BtnBetal_Click;

            Controls.Add(maximizeButton);
        }

 
        private void InitializeFormSettings()
        {

            KeyPreview = true;
            KeyPress += Form1_KeyPress;
            WindowState = FormWindowState.Maximized;
            Click += HandleInput;
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
                dataGridView1.Columns[3].Width = 110;
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

        
    }


}

