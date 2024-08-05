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
         
        public string Name { get; set; } 
        public string Barcode { get; set; } 
        public decimal Price { get; set; } 
        
        public int? ProductTypeID { get; set; }
        
      
       
    }

}
