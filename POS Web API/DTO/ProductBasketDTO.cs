namespace POS_API.DTO
{
     public class ProductBasketDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Barcode { get; set; }

        public decimal Price { get; set; }
        
    }
}
