using APIWebManagement.Data.Entities;
using APIWebManagement.Services.Interfaces;
using APIWebManagement.Utilities;
using APIWebManagement.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APIWebManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly UserManager<User> _userManager;
        public UsersController(IUserService userService, IRoleService roleService, UserManager<User> userManager)
        {
            _userService = userService;
            _roleService = roleService;
            _userManager = userManager;
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

            //if (!string.IsNullOrEmpty(userUpdateRequest.RoleName))
            //{
            //    var role = _roleService.GetRoleByName(userUpdateRequest.RoleName);
            //    if (role.Result != null)
            //    {
            //        var result = await _userManager.AddToRoleAsync(userUpdate, userUpdateRequest.RoleName);
            //        if (!result.Succeeded)
            //        {
            //            var error = result.Errors.ToList();
            //            var lstError = new List<string>();
            //            foreach (var item in error)
            //            {
            //                lstError.Add(item.Description);
            //            }

            //            return Ok(new MessageResponse(false, string.Join(",", lstError)));
            //        }
            //    }
            //}
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

        [HttpGet("RoleUser")]
        public async Task<IActionResult> GetRoleUser()
        {
            var userRole = await _userService.GetUserWithRoles();
            return Ok(userRole);
        }


        [HttpPost("EditRoles/{userName}")]
        public async Task<IActionResult> GetRoleUser(string userName, UserEditRoleRequest request)
        {
            var result = await _userService.EditRoleUser(userName, request);
            return Ok(new { Message = "Updated Role User successfully", RoleName = result });
        }
    }
}
