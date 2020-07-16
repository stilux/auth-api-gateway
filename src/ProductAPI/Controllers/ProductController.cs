using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using ProductAPI.Models;
using ProductAPI.Models.Errors;
using ProductAPI.Providers;

namespace ProductAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductContext _context;

        private readonly IMemoryCache _cache;

        private readonly CacheConfig _cacheConfig;

        public ProductController(ProductContext context, IMemoryCache cache, IOptions<CacheConfig> config)
        {
            _context = context;
            _cache = cache;
            _cacheConfig = config.Value;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> Get(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
                return Ok(product);
            return NotFound();
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Product>> Create([FromBody] Product product)
        {
            if (product == null)
                return BadRequest();

            _context.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new {id = product.Id}, product);
        }

        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(int id, [FromBody] Product updatedProduct)
        {
            if (id != updatedProduct.Id)
            {
                return BadRequest(new ErrorModel {FieldName = "Id", Message = "Invalid product id"});
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            product.Name = updatedProduct.Name;
            product.Vendor = updatedProduct.Vendor;
            product.VendorCode = updatedProduct.VendorCode;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            var user = await _context.Products.FindAsync(id);
            if (user == null)
                return NotFound();

            _context.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("find")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Product>>> Find([FromQuery] string searchPattern, [FromQuery] int take = 100)
        {
            if (string.IsNullOrWhiteSpace(searchPattern))
                return BadRequest($"Parameter {nameof(searchPattern)} is empty.");

            var cacheEnabled = _cacheConfig.Enabled;
            IEnumerable<Product> products = null;
            
            if (cacheEnabled)
            {
                var key = $"{nameof(Find)}_{searchPattern.ToLower()}";
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
            return Ok(products.Take(take));
        }

        private async Task<IList<Product>> FindInternalAsync(string searchPattern)
        {
            var pattern = $"%{searchPattern}%";
            return await _context.Products
                .Where(p => EF.Functions.ILike(p.Name, pattern) 
                            || EF.Functions.ILike(p.Vendor, pattern)
                            || EF.Functions.ILike(p.VendorCode, pattern))
                .ToListAsync();
        }
    }
}