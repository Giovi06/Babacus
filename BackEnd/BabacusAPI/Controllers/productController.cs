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

    /// <summary>
    /// Represents a controller for managing products in the Babacus API.
    /// </summary>
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
            var products = await _context.Products.Select(p => ProductToNewProductDTO(p)).ToListAsync();
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
            return Ok(ProductToNewProductDTO(product));
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

                var price = boughtProduct.Price * boughtProduct.Quantity;
                amount += price;
                // Check if the product already exists in the database
                var existingProduct = await CheckIfProductExists(boughtProduct);
                if (existingProduct != null)
                {
                    existingProduct.Stock += boughtProduct.Quantity;
                    _context.Products.Update(existingProduct);
                    continue;
                }
                else
                {
                    // Create the bought product using the provided data
                    var newBoughtProduct = ProductDTOToNewProduct(boughtProduct);
                    newBoughtProduct.Stock = boughtProduct.Quantity;
                    _context.Products.Add(newBoughtProduct);
                    continue;
                }
            }
            if (amount != request.Payment.Amount)
            {
                return BadRequest($"Invalid payment amount. Check the bought products prices. The total amount: ${amount} is not equal to the payment amount: ${request.Payment.Amount}.");
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductById), new
            {
                request.BoughtProductsList[0].Id
            }, request.BoughtProductsList[0]);
        }
        [HttpPost]
        [Route("soldproducts")]
        public IActionResult SoldProducts(SoldProductRequest req)
        {
            if (req == null)
            {
                return BadRequest("Request body is null.");
            }

            if (req.SoldProductsList == null || req.SoldProductsList.Count == 0)
            {
                return BadRequest("SoldProductsList is empty.");
            }

            foreach (var soldProduct in req.SoldProductsList)
            {
                if (soldProduct.Id < 0 || soldProduct.Quantity <= 0)
                {
                    return BadRequest("Invalid sold product data.\n Product ID and Quantity are required.");
                }

            }

            if (string.IsNullOrEmpty(req.Payment.Method))
            {
                return BadRequest("Payment method is required.");
            }

            if (req.Payment.Amount <= 0)
            {
                return BadRequest("Invalid payment amount.");
            }


            return CreatedAtAction(nameof(GetProductById), new
            {
                req.SoldProductsList[0].id
            }, req.SoldProductsList[0]);

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
        private static ProductDTO ProductToNewProductDTO(Product p)
        {
            return new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                SupplierId = p.SupplierId,
                Stock = p.Stock
            };
        }
        private static Product ProductDTOToNewProduct(ProductDTO p)
        {
            var product = new Product
            {
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                SupplierId = p.SupplierId,
                Stock = p.Stock
            };

            return product;
        }

        /// <summary>
        /// Checks if a product already exists in the database based on its name.
        /// </summary>
        /// <param name="product">The product to check.</param>
        /// <param name="_context">The database context.</param>
        /// <returns>Returns a ProductDTO object if the product already exists, otherwise returns null.</returns>
        private async Task<Product?> CheckIfProductExists(ProductDTO productDTO)
        {
            if (await _context.Products.AnyAsync(p => p.Name == productDTO.Name))
            {
                var existingProduct = await _context.Products.FirstAsync(p => p.Name == productDTO.Name);
                return existingProduct;
            }
            return null;
        }

        public class BoughtProductRequest
        {
            public required List<ProductDTO> BoughtProductsList { get; set; }
            public required Payment Payment { get; set; }
        }
        public class SoldProductRequest
        {
            public required List<ProductDTO> SoldProductsList { get; set; }
            public required Payment Payment { get; set; }
        }
        public class Payment
        {
            public required string Method { get; set; }
            public required decimal Amount { get; set; }
        }
    }

}
