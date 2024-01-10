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
		[HttpPost("createUser/{roleName}")]
		public async Task<IActionResult> RegisterUser([FromBody] CreateUserDto model, [FromRoute] string roleName)
		{
			RequestResultDto requestResultDto = await _userService.CreateUser(model, roleName);
			if (!requestResultDto.result)
			{
				return BadRequest(requestResultDto.message);
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
					return Unauthorized(loginResultDto.message);
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
		[HttpPut("updateOwnPassword")]
		public async Task<IActionResult> UpdatePassword([FromBody] string model)
		{
			try
			{
				bool result = await _userService.UpdateCurrentUserPassword(model);
				if (result)
				{
					return Ok();
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
		[HttpPatch("updateOwnName")]
		public async Task<IActionResult> UpdateName([FromBody] JsonPatchDocument<UpdateUserNameDto> model)
		{
			try
			{
				bool result = await _userService.UpdatCurrentUserName(model);
				if (result)
				{
					return Ok();
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
		[HttpGet("getOwnUserData")]
		public async Task<IActionResult> GetUserData()
		{
			try
			{
				ResponseUserDto result = await _userService.GetCurrentUserInformation();
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
		[HttpPut("changeUserRole/{userID}")]
		public async Task<IActionResult> ChangeRole([FromBody] string roleName, int userID)
		{
			try
			{
				RequestResultDto requestResultDto = await _userService.UpdateUserRole(roleName, userID);
				if (requestResultDto.result)
				{
					return Ok();
				}
				else
				{
					return BadRequest(requestResultDto.message);
				}
			}
			catch
			{
				return BadRequest("Kullanıcı rolü güncellenmesinde hata oluştu");
			}
		}


	}
}
