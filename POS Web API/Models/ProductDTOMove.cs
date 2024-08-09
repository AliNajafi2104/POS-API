namespace POS_API.Models
{
    public class ProductDTOMove
    {
        public string Name { get; set; }
        public string Barcode { get; set; }
        public decimal Price { get; set; }

        public int? Count { get; set; }

        public DateTime? CountDate { get; set; }
    }
}
