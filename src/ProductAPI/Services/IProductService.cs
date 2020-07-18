using System.Collections.Generic;
using System.Threading.Tasks;
using ProductAPI.Models;

namespace ProductAPI.Services
{
    public interface IProductService
    {
        Task<Product> GetAsync(int id);
        Task CreateAsync(Product product);
        Task<bool> UpdateAsync(Product updatedProduct);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Product>> FindAsync(string searchPattern, int take = 100);
    }
}