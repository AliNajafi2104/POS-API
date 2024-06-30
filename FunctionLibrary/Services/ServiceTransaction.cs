using FunctionLibrary.DTO;
using FunctionLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionLibrary.Services
{
    public class ServiceTransaction : IServiceTransaction
    {

        private readonly DataContext _context;


        public ServiceTransaction(DataContext context)
        {
            _context = context;
        }


        public async Task RegisterTransaction(transactionDTO transaction)
        {
            try
            {
                // Determine the current date in yyyyMMdd format
                string currentDate = DateTime.Now.ToString("yyyyMMdd");

                // Query the database to find transactions matching the initial criteria
                var transactions = await _context.Transaction_
                    .Where(t => t.TransactionID.StartsWith(currentDate))
                    .ToListAsync();

                // Determine the next sequential number
                int nextNumber = 1;
                if (transactions.Any())
                {
                    // Extract the sequential number part from the latest TransactionID
                    var lastTransaction = transactions
                        .OrderByDescending(t => t.TransactionID)
                        .FirstOrDefault();

                    string lastNumberString = lastTransaction.TransactionID.Substring(9); // Assuming format is "yyyyMMdd-NNNN"
                    if (int.TryParse(lastNumberString, out int lastNumber))
                    {
                        nextNumber = lastNumber + 1;
                    }
                }

                // Generate the new TransactionID
                string nextTransactionID = $"{currentDate}-{nextNumber.ToString("0000")}";

                // Create a new Transaction object
                Transaction transaction1 = new Transaction
                {
                    TransactionID = nextTransactionID,
                    CVR = "CVR-NUMMER",
                    ActionType = transaction.ActionType,
                    StartTimestamp = transaction.StartTimestamp,
                    EndTimestamp = transaction.EndTimestamp,
                    PaymentMethodID = transaction.PaymentMethodID,
                    SystemSerialNumber = "SSN",
                    DigitalSignature = "SIGNATURE",
                    Amount = transaction.Amount,
                    SalespersonID = transaction.SalespersonID
                };

                // Add the transaction to the context and save changes
                _context.Transaction_.Add(transaction1);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error registering transaction", ex);
            }
        }

        


public async Task<List<Transaction>> GetTransactions()
        {
            var transactions = await _context.Transaction_.ToListAsync();
            return transactions;
        }



    }
}
