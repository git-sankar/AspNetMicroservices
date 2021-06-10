using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository productRepository;

        private readonly ILogger<CatalogController> logger;

        public CatalogController(IProductRepository productRepository, ILogger<CatalogController> logger)
        {
            this.productRepository = productRepository;
            this.logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await productRepository.GetProducts();
            return Ok(products);
        }

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var product = await productRepository.GetProduct(id);
            if (product == null)
            {
                logger.LogError($"Product with id : {id}, not found");
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet("action/{Category}", Name = "GetProductByCategory")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        //[ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
        {
            var products = await productRepository.GetProductByCategory(category);
            //if(products == null)
            //{
            //    logger.LogError($"There are no products available for the catgory {category}.");
            //    return NotFound();
            //}
            return Ok(products);

        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await productRepository.CreateProduct(product);

            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            var status = await productRepository.UpdateProduct(product);

            return Ok(status);
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            return Ok(await productRepository.DeleteProduct(id));
        }
    }
}
