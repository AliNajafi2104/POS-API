namespace POS_API.DTO
{
    public class ProductDTO
    {
        public string? Name { get; set; }

        public string Barcode { get; set; }

        public decimal? Price { get; set; }
        public int Amount { get; set; }

    }
}
