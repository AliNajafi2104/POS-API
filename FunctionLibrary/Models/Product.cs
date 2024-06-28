using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionLibrary.Models
{
    public class Product
    {
        public int ProductID { get; set; } // Primary key
        public string Name { get; set; } // Not null
        public string Barcode { get; set; } // Nullable
        public decimal? Price { get; set; } // Nullable
        public bool PricedByWeight { get; set; } // Not null
       
        public decimal? PricePerUnitWeight { get; set; } // Nullable
        public string Category { get; set; } // Not null
    }

}
