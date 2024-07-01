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
               
                string currentDate = DateTime.Now.ToString("yyyyMMdd");

               
                var transactions = await _context.Transaction_
                    .Where(t => t.TransactionID.StartsWith(currentDate))
                    .ToListAsync();

               
                int nextNumber = 1;
                if (transactions.Any())
                {
                    
                    var lastTransaction = transactions
                        .OrderByDescending(t => t.TransactionID)
                        .FirstOrDefault();

                    string lastNumberString = lastTransaction.TransactionID.Substring(9); 
                    if (int.TryParse(lastNumberString, out int lastNumber))
                    {
                        nextNumber = lastNumber + 1;
                    }
                }

                
                string nextTransactionID = $"{currentDate}-{nextNumber.ToString("0000")}";

               
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
