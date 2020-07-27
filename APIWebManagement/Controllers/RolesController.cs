using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIWebManagement.Services.Interfaces;
using APIWebManagement.Utilities;
using APIWebManagement.ViewModels.Role;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APIWebManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        // GET: api/<RolesController>
        [HttpGet]
        public IActionResult GetAllRole()
        {
            var roles = _roleService.GetAllRole();
            if (roles != null)
            {
                var lstRole = roles.ToList();
                return Ok(new { Data = lstRole });
            }

            return BadRequest();
        }

        // GET api/<RolesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var role = await _roleService.GetRoleById(id);
            if (role == null)
                return BadRequest();

            return Ok(role);
        }

        // POST api/<RolesController>
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] RequestCreateRole request)
        {
            var roleID = await _roleService.CreateRole(request);
            if (roleID == 0)
                return BadRequest();

            var newRole = await _roleService.GetRoleById(roleID);

            return CreatedAtAction(nameof(GetById), new { id = roleID }, newRole);
        }

        //PUT api/<RolesController>/5
        [HttpPut]
        public async Task<IActionResult> UpdatedRole(RoleUpdateRequest request)
        {
            var result = await _roleService.UpdatedRole(request);
            if (result == 0)
                return BadRequest();

            return Ok(new MessageResponse("Updated Role successfully"));
        }

        // DELETE api/<RolesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var result = await _roleService.DeleteRole(id);
            if (result == 0)
                return BadRequest();

            return Ok(new MessageResponse("Deleted Role successfully"));

        }
    }
}
