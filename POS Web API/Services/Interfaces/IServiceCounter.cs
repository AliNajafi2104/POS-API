namespace POS_API.Services.Interfaces
{
    public interface IServiceCounter
    {
        Task AddProductCount(Product product);

        Task ResetProductCounters();

        Task<decimal> GetTotalPriceOfCounters();
    }
}
