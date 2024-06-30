using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FunctionLibrary.DTO;
using FunctionLibrary.Models;
namespace FunctionLibrary.Services
{
    public interface IServiceTransaction
    {
        public  Task RegisterTransaction(transactionDTO transaction);

        public Task<List<Transaction>> GetTransactions();


    }
}
