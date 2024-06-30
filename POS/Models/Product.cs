using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionLibrary.Models
{
    public class Product
    {
       

        public Product()
        {
            Antal = 1;
        }

        public string Name { get; set; } // Not null
        public string Barcode { get; set; } // Nullable
        public decimal Price { get; set; } // Nullable

        public int ProductTypeID { get; set; }
        public int Antal;
      
       
    }

}
