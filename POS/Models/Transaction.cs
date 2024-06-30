using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace searchengine123.Models
{
    public class Transaction
    {

      

        public string TransactionID { get; set; }
        public string CVR { get; set; }
        public string ActionType { get; set; }
        public DateTime StartTimestamp { get; set; }
        public DateTime EndTimestamp { get; set; }
        public int PaymentMethodID { get; set; }
        public string SystemSerialNumber { get; set; }
        public string DigitalSignature { get; set; }
        public decimal Amount { get; set; }
        public int SalespersonID { get; set; }

    }
}
