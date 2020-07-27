using APIWebManagement.Services.Interfaces;
using APIWebManagement.ViewModels.Login;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APIWebManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthsController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST api/<AuthsController>
        [HttpPost("Login")]
        public async Task<IActionResult> Post([FromBody] UserForLoginRequest request)
        {
            var result = await _authService.Login(request);
            if(result != null)
                return Ok(result);

            return BadRequest();
        }

        

    }
}
