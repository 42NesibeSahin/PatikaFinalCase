using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Application.DTOs;
using WebAPI.Application.Helpers.UserHelpers;
using WebAPI.Application.Interfaces.Services;
using WebAPI.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserService _userService;

        public UserController(IUserService userService, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [Authorize(Roles = nameof(UserRoleEnum.admin))]
        [HttpPost("createUser/{roleID}")]
        public async Task<IActionResult> RegisterUser([FromBody] CreateUserDto model, [FromRoute] int roleID)
        {
            try
            {
                int createdRoleID = await _userService.CreateUser(model, roleID);
            }
            catch (Exception ex)
            {
                return BadRequest("Kullanıcı oluştururken hata oluştu");
            }
            return Ok(new { Message = "Kullanıcı başarıyla oluşturuldu." });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto model)
        {
            try
            {
                LoginResultDto loginResultDto = await _userService.LoginUser(model);
                if (!loginResultDto.result)
                {
                    return Unauthorized();
                }
                else
                {
                    return Ok(new { Token = loginResultDto.Token });
                }
            }
            catch
            {
                return Unauthorized();
            }
        }

        [Authorize]
        [HttpPut("updatePassword/{userID}")]
        public async Task<IActionResult> UpdatePassword([FromBody] string model, [FromRoute] int userID)
        {
            try
            {
                bool result = await _userService.UpdateUserPassword(model, userID);
                if (result)
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest("Şifreniz güncellenemedi");
                }
            }
            catch
            {
                return BadRequest("Şifreniz güncellemede hata oluştu");
            }
        }

        [Authorize]
        [HttpPatch("updateName/{userID}")]
        public async Task<IActionResult> UpdateName([FromBody] JsonPatchDocument<UpdateUserNameDto> model, [FromRoute] int userID)
        {
            try
            {
                bool result = await _userService.UpdateUserName(model, userID);
                if (result)
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest("Bilgileriniz güncellenemedi");
                }
            }
            catch
            {
                return BadRequest("Bilgilerinizin güncellenmesinde hata oluştu");
            }
        }


        [Authorize]
        [HttpGet("getUser/{userID}")]
        public async Task<IActionResult> GetUserData(int userID)
        {
            try
            {
                ResponseUserDto result = await _userService.GetUserInformation(userID);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NoContent();
                }
            }
            catch
            {
                return BadRequest("Bilgileriniz getirmede hata oluştu");
            }
        }

        [Authorize(Roles = nameof(UserRoleEnum.admin))]
        [HttpPut("changeRole/{userID}")]
        public async Task<IActionResult> ChangeRole([FromBody] int roleID, int userID)
        {
            try
            {
                bool result = await _userService.UpdateUserRole(roleID, userID);
                if (result)
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest("Kullanıcı rolü güncellenemedi");
                }
            }
            catch
            {
                return BadRequest("Kullanıcı rolü güncellenmesinde hata oluştu");
            }
        }


    }
}
