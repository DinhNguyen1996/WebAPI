﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIWebManagement.Data.Entities;
using APIWebManagement.Services.Interfaces;
using APIWebManagement.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APIWebManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        // GET: api/<ProductsController>
        [HttpGet]
        public async Task<IActionResult> GetAllCategory([FromQuery] GetCategoriesPagingRequest request)
        {
            var lstCategories = await _categoryService.GetAllCategory(request);

            return Ok(lstCategories);
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetById(id);
            if (category == null)
                return BadRequest();

            return Ok(category);
        }

        // POST api/<ProductsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CategoryCreateRequest categoryCreateRequest)
        {
            var newCategoryID = await _categoryService.CreateCategory(categoryCreateRequest);
            if (newCategoryID == 0)
                return BadRequest();

            var category = await _categoryService.GetById(newCategoryID);

            return CreatedAtAction(nameof(GetById), new { id = newCategoryID }, category);
        }

        // PUT api/<ProductsController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] CategoryUpdateRequest categoryUpdateRequest)
        {
            var result = await _categoryService.UpdateCategory(categoryUpdateRequest);
            if (result == 0)
                return BadRequest();

            return Ok(new { Message = "Updated Category successfully" });
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.DeleteCategory(id);
            if (result == 0)
                return BadRequest();

            return Ok(new { Message = "Deleted Category successfully" });
        }
    }
}
