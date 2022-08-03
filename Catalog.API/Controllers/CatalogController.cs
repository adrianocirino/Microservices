using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public CatalogController(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw  new ArgumentException(nameof(productRepository));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _productRepository.GetProducts();
            if (products is null) return NoContent();
            
            return Ok(products);
        }

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Product), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product is null) return NotFound();

            return Ok(product);
        }

        [Route("[action]/{category}", Name = "GetProductByCategory")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
        {
            if (category is null) return BadRequest();
            
            var product = await _productRepository.GetProductByCategory(category);
            if (product is null) return NotFound();

            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            if (product is null) return BadRequest();

            await _productRepository.CreateProduct(product);

            return CreatedAtRoute("GetProduct", new { product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Product>> UpdateProduct(Product product)
        {
            if (product is null) return BadRequest();

            return Ok(await _productRepository.UpdateProduct(product));
        }

        [HttpDelete]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Product>> DeleteProduct(string id)
        {
            if (id is null) return BadRequest();

            var product = await _productRepository.GetProductById(id);

            if (product is null) return NotFound();

            return Ok(_productRepository.DeleteProduct(id));
        }
    }
}
