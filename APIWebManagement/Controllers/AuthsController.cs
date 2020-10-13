using APIWebManagement.Data.Entities;
using APIWebManagement.Services.Interfaces;
using APIWebManagement.Utilities;
using APIWebManagement.ViewModels.Login;
using EmailService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
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
        private readonly IWebHostEnvironment _hostingEnvironment;

        public AuthsController(IAuthService authService, UserManager<User> userManager, IWebHostEnvironment hostingEnvironment)
        {
            _authService = authService;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }

        // POST api/<AuthsController>
        [HttpPost("Login")]
        public async Task<IActionResult> Post([FromBody] UserForLoginRequest request)
        {
            var result = await _authService.Login(request);
            if (result != null)
                return Ok(result);

            return BadRequest();
        }

        [HttpGet("SendEmail")]
        public IActionResult SendEmail()
        {
            try
            {
                string rootPath = _hostingEnvironment.ContentRootPath;
                string BodyTemplate = WebTemplateHelper.GetTemplateContent(rootPath, "Email/ActiveAccount.html");
                //replace
                //var logo = _configuration["AppSettings:WebApi"] + "/Upload/Default/logo.png";
                HttpContext.SendEmail(
                          EmailTo: "thanhdinhbmt123@gmail.com",
                          SubjectContent: "Kích hoạt tài khoản",
                          BodyContent: BodyTemplate
                        );

                return Ok();
            }
            catch (Exception ex)
            {
                throw new WebManagementException("Lỗi", ex);
            }

        }

    }
}
