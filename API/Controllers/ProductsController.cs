using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductRepository ProductRepository) : ControllerBase
    {

        [HttpGet] //GET api/products
        public async Task<ActionResult<IReadOnlyList<Product>>> GetAllProducts()
        {
            var AllProducts = await ProductRepository.GetAllProductsAsync();
            return Ok(AllProducts);
        }

        [HttpGet]
        [Route("{id:int}")] //GET api/products/id
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await ProductRepository.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return product;
        }

        [HttpPost] //Post api/products
        public async Task<ActionResult<Product>> AddProduct(Product product)
        {
            ProductRepository.AddProduct(product);
            if (await ProductRepository.SaveChangesAsync())
            {
                return CreatedAtAction("GetProduct", new { id = product.Id }, product);
            }
            return BadRequest("Error occured while Adding the new product");
        }

        [HttpPut] //PUT api/products
        [Route("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, Product newProduct)
        {
            //check if the id sent is matching the id of the entity
            if (id != newProduct.Id)
            {
                return BadRequest("Product ID mismatch");
            }
            //get the old product 
            var OldProduct = await ProductRepository.GetProductByIdAsync(id);
            //make sure that it exist in the database
            if (OldProduct == null) return NotFound($"Product with ID {id} not found");

            //update the product in the database 
            ProductRepository.UpdateProduct(OldProduct, newProduct);

            //save the changes 
            if (await ProductRepository.SaveChangesAsync())
            {
                return NoContent();
            }

            return BadRequest("Error occured while updating the product");
        }


        [HttpDelete("{id}")]//Delete api/product/{id}
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await ProductRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound($"Product with ID {id} not found");
            }

            ProductRepository.DeleteProduct(product);
            if (await ProductRepository.SaveChangesAsync())
            {
                return NoContent();
            }
            return BadRequest("Error occured while Deleting the product"); ;
        }
    }
}
