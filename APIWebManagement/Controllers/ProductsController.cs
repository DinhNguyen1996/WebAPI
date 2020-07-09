using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Schema;
using APIWebManagement.Data.Entities;
using APIWebManagement.Services.Interfaces;
using APIWebManagement.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APIWebManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        // GET: api/<ProductsController>
        [HttpGet]
        public async Task<IActionResult> GetAllProduct([FromQuery] GetProductsPagingRequest request)
        {
            var lstProducts = await _productService.GetAllProduct(request);

            return Ok(lstProducts);
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetById(id);
            if (product == null)
                return BadRequest();

            return Ok(product);
        }

        // POST api/<ProductsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductCreateRequest productCreateRequest)
        {
            var newProductID = await _productService.CreateProduct(productCreateRequest);
            if (newProductID == 0)
                return BadRequest();

            var newProduct = await _productService.GetById(newProductID);

            return CreatedAtAction(nameof(GetById), new { id = newProductID }, newProduct);
        }

        // PUT api/<ProductsController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ProductUpdateRequest productUpdateRequest)
        {
            var result = await _productService.UpdateProduct(productUpdateRequest);
            if (result == 0)
                return BadRequest();

            return Ok(new { Message = "Updated Product successfully" });
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.DeleteProduct(id);
            if (result == 0)
                return BadRequest();

            return Ok(new { Message = "Deleted Product successfully" });
        }
    }
}
