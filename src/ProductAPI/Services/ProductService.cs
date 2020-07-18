using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using ProductAPI.Exceptions;
using ProductAPI.Models;
using ProductAPI.Providers;

namespace ProductAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductContext _context;
        
        private readonly IMemoryCache _cache;

        private readonly CacheConfig _cacheConfig;

        public ProductService(ProductContext context, IMemoryCache cache, IOptions<CacheConfig> config)
        {
            _context = context;
            _cache = cache;
            _cacheConfig = config.Value;
        }
        
        public async Task<Product> GetAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }
        
        public async Task CreateAsync(Product product)
        {
            _context.Add(product);
            await _context.SaveChangesAsync();
        }
        
        public async Task<bool> UpdateAsync(Product updatedProduct)
        {
            var product = await _context.Products.FindAsync(updatedProduct.Id);
            if (product == null)
                throw new NotFoundException();

            product.Name = updatedProduct.Name;
            product.Vendor = updatedProduct.Vendor;
            product.VendorCode = updatedProduct.VendorCode;

            return await _context.SaveChangesAsync() > 0;
        }
        
        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Products.FindAsync(id);
            if (user == null)
                throw new NotFoundException();

            _context.Remove(user);
            return await _context.SaveChangesAsync() > 0;
        }
        
        public async Task<IEnumerable<Product>> FindAsync(string searchPattern, int take = 100)
        {
            var cacheEnabled = _cacheConfig.Enabled;
            IEnumerable<Product> products = null;
            
            if (cacheEnabled)
            {
                var key = $"{nameof(FindAsync)}_{searchPattern.ToLower()}";
                products = await _cache.GetOrCreateAsync(key, entry =>
                {
                    entry.SetOptions(_cacheConfig.Options);
                    return FindInternalAsync(searchPattern);
                });
            }
            else
            {
                products = await FindInternalAsync(searchPattern);
            }
            return products.Take(take);
        }

        private async Task<IList<Product>> FindInternalAsync(string searchPattern)
        {
            var pattern = $"%{searchPattern}%";
            return await _context.Products
                .AsNoTracking()
                .Where(p => EF.Functions.ILike(p.Name, pattern) 
                            || EF.Functions.ILike(p.Vendor, pattern)
                            || EF.Functions.ILike(p.VendorCode, pattern))
                .ToListAsync();
        }
    }
}