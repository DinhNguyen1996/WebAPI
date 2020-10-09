using APIWebManagement.Data.Entities;
using APIWebManagement.Services.Interfaces;
using APIWebManagement.Utilities;
using APIWebManagement.ViewModels.Login;
using APIWebManagement.ViewModels.Sesstion;
using EmailService;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;

        public AuthsController(IAuthService authService, UserManager<User> userManager, IEmailSender emailSender)
        {
            _authService = authService;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        // POST api/<AuthsController>
        [HttpPost("Login")]
        public async Task<IActionResult> Post([FromBody] UserForLoginRequest request)
        {
            var result = await _authService.Login(request);
            if (result != null)
            {
                var sesstionLogin = new SesstionWeb
                {
                    UserID = result.User.UserID,
                    UserName = result.User.UserName
                };
                SessionHelper.SetObjectAsJson(HttpContext.Session, "SesstionLogin", sesstionLogin);
                return Ok(result);
            }

            return BadRequest();
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("SesstionLogin");
            //HttpContext.Session.Clear();
            //HttpContext.SignOutAsync();
            return Ok("Thoát thành công");
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel model)
        {

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                throw new WebManagementException("Can not find user");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return Ok();
        }

        [HttpGet("SendEmail")]
        public IActionResult SendEmail()
        {

            var message = new Message(new string[] { "thanhdinhbmt123@gmail.com" }, "Test email", "This is the content from our email");
            _emailSender.SendEmail(message);
            return Ok();
        }

    }
}
