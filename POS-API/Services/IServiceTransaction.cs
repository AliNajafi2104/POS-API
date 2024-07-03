using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public interface IServiceTransaction
    {
        public  Task RegisterTransaction(transactionDTO transaction);

        public Task<List<Transaction>> GetTransactions();


        public Task RegisterTransactionDetails(List<Product> products);


    }
