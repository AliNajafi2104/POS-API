using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionLibrary.Models
{
    public class Journal
    {
        public int JournalID { get; set; }
        public DateTime ChangeTimestamp { get; set; }
        public string ChangeType { get; set; }
        public string Details { get; set; }
    }

}
