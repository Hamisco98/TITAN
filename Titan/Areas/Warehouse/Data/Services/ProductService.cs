using Titan.Areas.Warehouse.Data.Interfaces;
using Titan.Areas.Warehouse.Models;

namespace Titan.Areas.Warehouse.Data.Services
{
    public class ProductService : IProductInterface
    {
        private readonly HttpClient httpClient;
        public ProductService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<IEnumerable<Products>> GETALL()
        {
            return await httpClient.GetFromJsonAsync<IEnumerable<Products>>("api/products");
        }
    }
}
