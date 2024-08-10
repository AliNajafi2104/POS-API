

namespace POS_API.Services
{
    public interface IServiceCounter
    {
        Task AddProductCount(Product product);

        Task ResetProductCounters();

        Task<decimal> GetTotalPriceOfCounters();
    }
}
