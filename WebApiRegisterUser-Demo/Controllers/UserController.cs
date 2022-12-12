using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApiRegisterUser_Demo.Commons;
using WebApiRegisterUser_Demo.Models;
using WebApiRegisterUser_Demo.Services;

namespace WebApiRegisterUser_Demo.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly RegisterCodeService _registerCodeService;

        public UserController(UserService userService, RegisterCodeService registerCodeService)
        {
            _userService = userService;
            _registerCodeService = registerCodeService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> PostRegisterUser([FromBody] UserModel user)
        {
            // check email had exist
            var dataUser = _userService.GetUserByEmail(user.email);
            if (dataUser != null && dataUser.active == 1)
            {
                return Ok(new Responce()
                {
                    message = "Email had registed",
                    result = new { email = user.email }
                });
            }
            // generate code
            var code = Common.RandomCode(10);
            // send mail
            if (!Common.SendMailRegisterCode(code))
            {
                return Ok(new Responce()
                {
                    message = "Email invalid",
                    result = new { email = user.email }
                });
            }
            if (dataUser != null)
            {
                await _userService.DeleteAsync(dataUser.id);
            }
            // save template data
            _userService.CreateUser(new User(user));
            // save register code
            await _registerCodeService.CreateAsync(user.email, code);
            return Ok(new Responce()
            {
                message = "Register Code has send.",
                result = new {email = user.email}
            });
        }

        [HttpPut("CheckCode/{email}")]
        public async Task<IActionResult> PutRegisterUser(string email, [FromBody] string code)
        {
            var record = _registerCodeService.GetByEmailCode(email, code);
            if (record != null)
            {
                await _userService.ValidateRegisterUser(email);
                return Ok(new Responce()
                {
                    message = "User actived",
                    result = new { email = email }
                });
            }
            return Ok(new Responce()
            {
                message = "Email not found",
                result = new { email = email }
            });
        }
    }
}
