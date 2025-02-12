using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly StoreContext _context;

        public ProductsController(StoreContext context)
        {
            this._context = context;
        }

        [HttpGet] //GET api/products
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            var AllProducts = await _context.Products.ToListAsync<Product>();
            if (AllProducts == null) return NotFound();
            return AllProducts;
        }

        [HttpGet]
        [Route("{id:int}")] //GET api/products/id
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.FindAsync<Product>(id);
            if (product == null) return NotFound();
            return product;
        }

        [HttpPost] //Post api/products
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        [HttpPut] //PUT api/products
        [Route("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            //check if the id sent is matching the id of the entity
            if (id != product.Id)
            {
                return BadRequest("Product ID mismatch");
            }
            //make sure that it exist in the database
            var OldProduct = await _context.FindAsync<Product>(id);
            if (OldProduct == null) return NotFound($"Product with ID {id} not found");

            //update the product in the database 
            OldProduct.Name = product.Name.IsNullOrEmpty() ? OldProduct.Name : product.Name;
            OldProduct.Description = product.Description.IsNullOrEmpty() ? OldProduct.Description : product.Description;
            OldProduct.PictureUrl = product.PictureUrl.IsNullOrEmpty() ? OldProduct.PictureUrl : product.PictureUrl;
            OldProduct.Type = product.Type.IsNullOrEmpty() ? OldProduct.Type : product.Type;
            OldProduct.Brand = product.Brand.IsNullOrEmpty() ? OldProduct.Brand : product.Brand;
            OldProduct.Price = product.Price < 0 ? OldProduct.Price : product.Price;
            OldProduct.QuantityInStock = product.QuantityInStock < 0 ? OldProduct.QuantityInStock : product.QuantityInStock;

            //save the changes 
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}")]//Delete api/product/{id}
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound($"Product with ID {id} not found");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
