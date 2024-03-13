using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using BabacusAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace BabacusAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly BabacusDb _context;
        public ProductController(BabacusDb context)
        {
            this._context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts()
        {
            var products = await _context.Products.Select(p => ProductToProductDTO(p)).ToListAsync();
            return Ok(products);
        }


        [HttpGet]
        public IActionResult GetProductById(int id)
        {
            var product = product.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            product.Add(product);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }
        [HttpPost]
        public IActionResult NewBoughtProduct(Product product)
        {
            product.Add(product);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }
        [HttpPost]
        public IActionResult NewSoldProduct(Product product)
        {
            product.Add(product);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProductInfo(int id, Product updatedProduct)
        {
            var product = product.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;
            product.Description = updatedProduct.Description;
            product.supplierId = updatedProduct.supplierId;
            return NoContent();
        }
        [HttpPut("{id}")]
        public IActionResult UpdateProductStock(int id, Product updatedProduct)
        {
            var product = product.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            product.Stock = updatedProduct.Stock;
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = product.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            product.Remove(product);
            return NoContent();
        }
        private object ProductToProductDTO(Product p)
        {
            throw new NotImplementedException();
        }
    }
}
