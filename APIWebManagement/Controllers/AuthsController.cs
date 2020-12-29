using APIWebManagement.Data.Entities;
using APIWebManagement.Services.Interfaces;
using APIWebManagement.Utilities;
using APIWebManagement.ViewModels.Email;
using APIWebManagement.ViewModels.Login;
using APIWebManagement.ViewModels.Sesstion;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Mail;
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
        public async Task<IActionResult> Login([FromBody] UserForLoginRequest request)
        {
            var result = await _authService.Login(request);
            if (result.Token != null && result.User != null)
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
            return Ok(new { mess = "Thoát thành công" });
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
        public IActionResult SendEmail([FromBody] Email model)
        {
            try
            {
                //string rootPath = _hostingEnvironment.ContentRootPath;
                //string BodyTemplate = WebTemplateHelper.GetTemplateContent(rootPath, "Email/ActiveAccount.html");
                ////replace
                ////var logo = _configuration["AppSettings:WebApi"] + "/Upload/Default/logo.png";
                //HttpContext.SendEmail(
                //          EmailTo: "thanhdinhbmt123@gmail.com",
                //          SubjectContent: "Kích hoạt tài khoản",
                //          BodyContent: BodyTemplate
                //        );
                MailMessage mm = new MailMessage();
                mm.To.Add(model.To);
                mm.Subject = model.Subject;
                mm.Body = model.Body;
                mm.From = new MailAddress("admin@gmail.com");
                mm.IsBodyHtml = false;

                SmtpClient smtp = new SmtpClient("smtp.googlemail.com");
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Credentials = new System.Net.NetworkCredential("admin@gmail.com", "password");
                smtp.TargetName = "STARTTLS/smtp.gmail.com";
                smtp.Send(mm);

                return Ok();
            }
            catch (Exception ex)
            {
                throw new WebManagementException("Lỗi không gởi được Email", ex);
            }

        }

    }
}
