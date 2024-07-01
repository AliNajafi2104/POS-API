using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionLibrary.Models
{
    public class SalesSummary
    {
        public int SummaryID { get; set; }
        public string ReportType { get; set; }
        public DateTime GeneratedTimestamp { get; set; }
        public decimal TotalAmount { get; set; }
        public int? SalespersonID { get; set; } 
    }

}
