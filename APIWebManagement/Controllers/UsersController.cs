using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIWebManagement.Services.Interfaces;
using APIWebManagement.Utilities;
using APIWebManagement.ViewModels.User;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APIWebManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        // GET: api/<UsersController>
        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery] GetUsersPagingRequest request)
        {
            var userList = await _userService.GetAllUser(request);

            return Ok(userList);
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
                return BadRequest();

            return Ok(user);
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserCreateRequest userCreateRequest)
        {
            var userID = await _userService.CreateUser(userCreateRequest);
            if (userID == 0)
                return BadRequest();

            var user = await _userService.GetUserById(userID);

            return CreatedAtAction(nameof(GetById), new { id = userID }, user);
        }

        // PUT api/<UsersController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UserUpdateRequest userUpdateRequest)
        {
            var result = await _userService.UpdateUser(userUpdateRequest);
            if (result == 0)
                return BadRequest();

            return Ok(new MessageResponse("Updated User successfully"));
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.DeleteUser(id);
            if (result == 0)
                return BadRequest();

            return Ok(new MessageResponse("Deleted User successfully"));
        }
    }
}
