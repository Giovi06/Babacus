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
            var products = await _context.Products.Select(p => MapProductToDTO(p)).ToListAsync();
            return Ok(products);
        }


        [HttpGet]
        [Route("getsingleproduct")]
        public async Task<IActionResult> GetProductById(int Id)
        {
            if (this._context.Products == null)
            {
                return this.Problem("Entity set 'Products' is null.");
            }
            var product = await this._context.Products.FindAsync(Id);
            if (product == null)
            {
                return NotFound("Product not found.");
            }
            return Ok(MapProductToDTO(product));
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
            return CreatedAtAction(nameof(GetProductById), new { Id = product.Id }, product);
        }
        [HttpPost]
        [Route("boughtproduct")]
        public async Task<IActionResult> BoughtProducts(ProductRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request body is null.");
            }

            // Validate the request body
            if (request.ProductsList == null || request.ProductsList.Count == 0)
            {
                return BadRequest("BoughtProductsList is empty.");
            }

            foreach (var boughtProduct in request.ProductsList)
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
            foreach (var boughtProduct in request.ProductsList)
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
                else if (existingProduct == null)
                {
                    // Create the bought product using the provided data
                    var newBoughtProduct = ProductDTOToNewProduct(boughtProduct);
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
                request.ProductsList[0].Id
            }, request.ProductsList[0]);
        }
        [HttpPost]
        [Route("soldproducts")]
        public async Task<IActionResult> SoldProducts(ProductRequest request)
        {
            // Validate the request body
            if (request == null)
            {
                return BadRequest("Request body is null.");
            }

            if (request.ProductsList == null || request.ProductsList.Count == 0)
            {
                return BadRequest("SoldProductsList is empty.");
            }

            foreach (var soldProduct in request.ProductsList)
            {
                if (soldProduct.Id < 0 || soldProduct.Quantity <= 0)
                {
                    return BadRequest("Invalid sold product data.");
                }
            }

            if (string.IsNullOrEmpty(request.Payment.Method))
            {
                return BadRequest("Payment method is required.");
            }

            decimal amount = request.Payment.Amount;
            decimal price = 0;

            // Process the request and update the sold products
            foreach (var soldProduct in request.ProductsList)
            {
                // Find the product by ID
                var product = _context.Products.FirstOrDefault(p => p.Id == soldProduct.Id);
                if (product == null)
                {
                    return NotFound($"Product with ID {soldProduct.Id} not found.");
                }

                price = product.Price * soldProduct.Quantity;

                // Update the product stock
                product.Stock -= soldProduct.Quantity;
                _context.Products.Update(product);

            }
            if (price != amount)
            {
                return BadRequest($"Invalid payment amount. Check the sold products prices. The total amount: ${price} is not equal to the payment amount: ${amount}.");
            }
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut]
        [Route("updateinfo")]
        public async Task<IActionResult> UpdateProductInfo(int Id, ProductDTO req)
        {
            if (req == null)
            {
                return BadRequest("Request body is null.");
            }
            if (req.Name == null || req.Price <= 0 || req.SupplierId <= 0 || req.Description == null)
            {
                return BadRequest("Invalid product data.");
            }

            var product = _context.Products.FirstOrDefault(p => p.Id == Id);


            if (product == null)
            {
                return NotFound($"The product with the id: {Id} does not exist.");
            }

            product.Name = req.Name;
            product.Price = req.Price;
            product.Description = req.Description;
            product.SupplierId = req.SupplierId;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPut]
        [Route("updatestock")]
        public async Task<IActionResult> UpdateProductStock(int Id, ProductDTO req)
        {
            if (req == null)
            {
                return BadRequest("Request body is null.");
            }
            if (Id < 0)
            {
                return BadRequest("Invalid product Id.");
            }
            var product = _context.Products.FirstOrDefault(p => p.Id == Id);
            if (product == null)
            {
                return NotFound($"The product with the id: {Id} does not exist.");
            }

            product.Stock += req.Quantity;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        [Route("deleteproduct")]
        public IActionResult DeleteProduct(int Id)
            
        {
            if (Id < 0)
            {
                return BadRequest("Invalid product Id.");
            }
            var product = _context.Products.FirstOrDefault(p => p.Id == Id);
            if (product == null)
            {
                return NotFound($"The product with the id: {Id} does not exist.");
            }

            _context.Products.Remove(product);
            _context.SaveChanges();
            return NoContent();
        }
        private static ProductDTO MapProductToDTO(Product p)
        {
            return new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                SupplierId = p.SupplierId,
                Quantity = 0,
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
                Stock = p.Stock // Use the null-coalescing operator to provide a default value of 0 when p.Stock is null
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

        public class ProductRequest
        {
            public required List<ProductDTO> ProductsList { get; set; }
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
