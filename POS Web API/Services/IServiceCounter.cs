using POS_API.DTO;

namespace POS_API.Services
{
    public interface IServiceCounter
    {
        Task AddProductCount(ProductDTO product);

        Task ResetProductCounters();

        Task<decimal> GetTotalPriceOfCounters();
    }
}
