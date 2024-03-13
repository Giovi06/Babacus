using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using BabacusAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

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
        [Route("getallproducts")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts()
        {
            var products = await _context.Products.Select(p => ProductToProductDTO(p)).ToListAsync();
            return Ok(products);
        }


        [HttpGet]
        [Route("getsingleproduct")]
        public async Task<IActionResult> GetProductById(int id)
        {
            if (this._context.Products == null)
            {
                return this.Problem("Entity set 'Products' is null.");
            }
            var product = await this._context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound("Product not found.");
            }
            return Ok(ProductToProductDTO(product));
        }

        [HttpPost]
        [Route("addproduct")]
        public IActionResult AddProduct(Product product)
        {
            if (product == null)
            {
                return BadRequest("Product is null.");
            }
            if (product.Name == null || product.Price <= 0 || product.Stock < 0)
            {
                return BadRequest("Invalid product data.");
            }
            _context.Products.Add(product);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }
        [HttpPost]
        [Route("boughtproduct")]
        public async Task<IActionResult> BoughtProducts(BoughtProductRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request body is null.");
            }

            // Validate the request body
            if (request.BoughtProductsList == null || request.BoughtProductsList.Count == 0)
            {
                return BadRequest("BoughtProductsList is empty.");
            }

            foreach (var boughtProduct in request.BoughtProductsList)
            {
                if (string.IsNullOrEmpty(boughtProduct.Name) || boughtProduct.Price <= 0 || boughtProduct.SupplierId <= 0 || boughtProduct.Quantity <= 0)
                {
                    return BadRequest("Invalid bought product data.");
                }
            }

            if (string.IsNullOrEmpty(request.Payment.Method))
            {
                return BadRequest("Payment method is required.");
            }
            decimal amount = 0;
            // Process the request and create the bought products
            foreach (var boughtProduct in request.BoughtProductsList)
            {
                // Create the bought product using the provided data
                var newBoughtProduct = ProductDTOToProduct(boughtProduct);

                var price = boughtProduct.Price * boughtProduct.Quantity;
                amount += price;

                _context.Products.Add(newBoughtProduct);
            }
            if (amount != request.Payment.Amount)
            {
                return BadRequest($"Invalid payment amount. Check the bought products prices. The total amount: ${amount} is not equal to the payment amount: ${request.Payment.Amount}.");
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductById), new
            {
                request.BoughtProductsList[0].id
            }, request.BoughtProductsList[0]);
        }
        [HttpPost]
        [Route("soldproducts")]
        public IActionResult SoldProducts(Product product)
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
        private static ProductDTO ProductToProductDTO(Product p)
        {
            return new ProductDTO
            {
                id = p.id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                SupplierId = p.SupplierId,
                Stock = p.Stock
            };
        }
        private static Product ProductDTOToProduct(ProductDTO p)
        {
            return new Product
            {
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                SupplierId = p.SupplierId,
                Stock = p.Stock
            };
        }

        public class BoughtProductRequest
        {
            public required List<ProductDTO> BoughtProductsList { get; set; }
            public required Payment Payment { get; set; }
        }
        public class Payment
        {
            public required string Method { get; set; }
            public required decimal Amount { get; set; }
        }
    }

}
