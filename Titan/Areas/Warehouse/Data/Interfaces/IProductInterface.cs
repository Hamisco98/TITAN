using Titan.Areas.Warehouse.Models;

namespace Titan.Areas.Warehouse.Data.Interfaces
{
    public interface IProductInterface
    {
        public Task<IEnumerable<Products>> GETALL();
    }
}
