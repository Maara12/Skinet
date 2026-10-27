using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route ("api/[Controller]")]
    public class ProductsController: ControllerBase
    {
        private readonly StoreContext storeContext;

        public ProductsController(StoreContext storeContext)
        {
            this.storeContext = storeContext;
        }


        [HttpGet] //api/Products
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await  storeContext.Products.ToListAsync();
        }


        [HttpGet ("{id:int}")] //api/Products/id
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            Product? product = await storeContext.Products.FindAsync(id);

            if(product == null) 
                return NotFound();
            else
                return product;
        }

        [HttpPost] //api/Products/id
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            storeContext.Products.Add(product);
            await storeContext.SaveChangesAsync();
            return product;
        }

        [HttpPut("{id:int}")] //api/Products/id

        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if(id != product.Id || ProductExists(id) == false) return BadRequest("Cannot Update Product");

            storeContext.Entry(product).State = EntityState.Modified;

            await storeContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")] //api/Products/id
        public async Task<ActionResult> DeleteProduct(int id)
        {
            Product? product = await storeContext.Products.FindAsync(id);

            if(product == null) return NotFound();

            storeContext.Products.Remove(product);

            await storeContext.SaveChangesAsync();

            return NoContent();
             
        }

        private bool ProductExists(int id)
        {
            return storeContext.Products.Any(x=>x.Id == id);
        }
    }
}