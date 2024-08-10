using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProductID { get; set; }
    public string Name { get; set; }
    public string Barcode { get; set; }
    public decimal Price { get; set; }
    public int? Amount { get; set; }

    public int? Count { get; set; }

   

}


