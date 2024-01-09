using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Application.DTOs;
using WebAPI.Application.Helpers.UserHelpers;
using WebAPI.Application.Interfaces.Repositories;
using WebAPI.Application.Interfaces.Services;
using WebAPI.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http;
using WebAPI.Domain.Common.Claim;

namespace WebAPI.Persistence.Services
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;
		IMapper _mapper;
		private readonly UserManager<User> _userManager;
		private readonly IConfiguration _configuration;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public UserService(IUserRepository userRepository, IMapper mapper, UserManager<User> userManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
		{
			_userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
			_httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
		}
		public async Task<RequestResultDto> CreateUserWithToken(CreateUserDto createUserDto, string roleName)
		{
			RequestResultDto requestResultDto = new RequestResultDto() { result = false };
			try
			{
				User user = _mapper.Map<User>(createUserDto);

				var userByUserName = await _userManager.FindByNameAsync(user.UserName);

				if (userByUserName != null)
				{
					requestResultDto.message = "Bu kullanıcı daha önceden eklenmiş";
					return requestResultDto;
				}

				var createResult = await _userManager.CreateAsync(user, createUserDto.Password);
				if (!createResult.Succeeded)
				{
					requestResultDto.message = string.Join(",", createResult.Errors.Select(x => x.Description).ToList());
					return requestResultDto;
				}

				if (!UserHelper.ExistRoleName(roleName))
				{
					requestResultDto.message = "Var olmayan rol eklenemez";
					return requestResultDto;
				}

				await _userManager.AddToRoleAsync(user, roleName);
				await _userRepository.Save();
				requestResultDto.result = true;
				return requestResultDto;
			}
			catch (Exception ex)
			{
				await _userRepository.Rollback();
				requestResultDto.message = "Kullanıcı oluştururken hata oluştu";
				return requestResultDto;
			}
		}

		public async Task<LoginResultDto> LoginUser(LoginUserDto loginUserDto)
		{
			LoginResultDto loginResultDto = new LoginResultDto() { result = false };

			var user = await _userManager.FindByNameAsync(loginUserDto.Username);
			if (user != null)
			{
				var result = await _userManager.CheckPasswordAsync(user, loginUserDto.Password);
				if (result)
				{
					loginResultDto.Token = await GenerateToken(user);
					loginResultDto.result = true;
				}
				else
				{
					loginResultDto.message = "Kullanıcı adı veya şifre doğru değil";
					return loginResultDto;
				}
			}
			else
			{
				loginResultDto.message = "Kullanıcı mevcut değil";
				return loginResultDto;
			}
			return loginResultDto;
		}

		public async Task<bool> UpdateCurrentUserPassword(string password)
		{

			bool result = false;
			var user = await GetCurrentUserAsync();
			if (user != null)
			{
				await _userManager.RemovePasswordAsync(user);
				if (!await _userManager.HasPasswordAsync(user))
				{
					await _userManager.AddPasswordAsync(user, password);
				}
				await _userRepository.Save();
				result = true;
			}
			return result;
		}

		public async Task<bool> UpdatCurrentUserName(JsonPatchDocument<UpdateUserNameDto> updateNameDto)
		{
			bool result = false;
			var user = await GetCurrentUserAsync();
			if (user != null)
			{
				var userPatch = _mapper.Map<UpdateUserNameDto>(user);
				updateNameDto.ApplyTo(userPatch);
				_mapper.Map(userPatch, user);

				await _userManager.UpdateAsync(user);
				result = true;
			}
			return result;
		}

		public async Task<ResponseUserDto> GetCurrentUserInformation()
		{
			var user = await GetCurrentUserAsync();
			ResponseUserDto responseUserDto = _mapper.Map<ResponseUserDto>(user);

			return responseUserDto;
		}

		public async Task<RequestResultDto> UpdateUserRole(string roleName, int userID)
		{
			RequestResultDto requestResultDto = new RequestResultDto() { result = false };

			if (!UserHelper.ExistRoleName(roleName))
			{
				requestResultDto.message = "Değiştirilmek istenen rol kayıtlı değil";
				return requestResultDto;
			}

			var user = await _userManager.FindByIdAsync(userID.ToString());
			if (user != null)
			{
				var currentRoles = await _userManager.GetRolesAsync(user);
				await _userManager.RemoveFromRolesAsync(user, currentRoles);
				var result = await _userManager.AddToRoleAsync(user, roleName);

				if (!result.Succeeded)
				{
					requestResultDto.message = "Gönderilen kullanıcının rolü değiştirilemedi";
					return requestResultDto;
				}

				requestResultDto.result = true;
			}
			else
			{
				requestResultDto.message = "Gönderilen kullanıcı bulunamadı";
				return requestResultDto;
			}
			return requestResultDto;
		}

		public int GetCurrentUserId()
		{
			var userId = _httpContextAccessor.HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == nameof(UserClaimModel.Id)).Value;
			if (userId == null)
			{
				throw new UnauthorizedAccessException();
			}
			return int.Parse(userId);
		}
		public async Task<User> GetCurrentUserAsync()
		{
			var userId = GetCurrentUserId();
			var user = await _userManager.FindByIdAsync(userId.ToString());
			if (user == null)
			{ throw new UnauthorizedAccessException(); }
			return user;
		}

		public async Task<List<string>> GetCurrentUserRolesAsync()
		{
			var user = await GetCurrentUserAsync();
			var roles = await _userManager.GetRolesAsync(user);
			return roles.ToList();
		}

		private async Task<string> GenerateToken(User user)
		{

			var roles = await _userManager.GetRolesAsync(user);
			var claims = new List<Claim>();
			claims.Add(new Claim(ClaimTypes.Name, user.UserName));
			claims.Add(new Claim(ClaimTypes.NameIdentifier, user.UserName));
			claims.Add(new Claim(nameof(UserClaimModel.Id), user.Id.ToString()));

			foreach (var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}

			var userClaims = await _userManager.GetClaimsAsync(user);
			claims.AddRange(userClaims);

			var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
			var tokenDesc = new SecurityTokenDescriptor
			{
				SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
				Expires = DateTime.UtcNow.AddHours(12),
				Subject = new ClaimsIdentity(claims),
				Issuer = _configuration["JWT:Issuer"],
				Audience = _configuration["JWT:Audience"]
			};

			var tokenHandler = new JwtSecurityTokenHandler();

			var token = tokenHandler.CreateToken(tokenDesc);

			return tokenHandler.WriteToken(token);
		}

	}
}



