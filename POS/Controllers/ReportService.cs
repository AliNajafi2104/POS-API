using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace searchengine123.Models
{
    public class ReportService
    {
        TransactionService SQL = new TransactionService();
        public async Task GenerateZReport()
        {
            try
            {
                List<Transaction> transactions = await SQL.GetTransactions();

                if (transactions != null && transactions.Any())
                {
                    // Filter transactions for current date
                    DateTime currentDate = DateTime.Now.Date;
                    transactions = transactions
                        .Where(t => t.StartTimestamp.Date == currentDate)
                        .OrderBy(t => t.StartTimestamp)
                        .ToList();

                    // Calculate total sales amount
                    decimal totalSalesAmount = transactions.Sum(t => t.Amount);

                    // Calculate transaction count
                    int transactionCount = transactions.Count;

                    // Example: Format the Z report
                    StringBuilder reportBuilder = new StringBuilder();
                    reportBuilder.AppendLine("Z REPORT");
                    reportBuilder.AppendLine($"Total Sales Amount: {totalSalesAmount:C}");
                    reportBuilder.AppendLine($"Transaction Count: {transactionCount}");
                    reportBuilder.AppendLine("Date: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    // Prompt user to save the report
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                    saveFileDialog.FileName = "ZReport"+DateTime.Now.ToString("yyyy-MM-dd")+ ".txt";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Get the selected file path
                        string filePath = saveFileDialog.FileName;

                        // Save report to the selected file path
                        File.WriteAllText(filePath, reportBuilder.ToString());

                        // Inform user that file has been saved
                        MessageBox.Show($"Z Report saved to:\n{filePath}", "Z Report Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // User canceled the save operation
                        Console.WriteLine("Save operation canceled by user.");
                    }
                }
                else
                {
                    Console.WriteLine("No transactions found for the current date.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating Z Report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        public void GenerateXReport()
        {

        }
    }
}
