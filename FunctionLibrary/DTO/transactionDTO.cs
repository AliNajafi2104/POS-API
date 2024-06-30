using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionLibrary.DTO
{
    public class transactionDTO
    {

        public string ActionType { get; set; }
        public DateTime StartTimestamp { get; set; }
        public DateTime EndTimestamp { get; set; }
        public int PaymentMethodID { get; set; }


        public decimal Amount { get; set; }
        public int SalespersonID { get; set; }
    }
}
