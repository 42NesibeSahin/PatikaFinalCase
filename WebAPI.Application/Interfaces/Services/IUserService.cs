using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Application.DTOs;
using WebAPI.Domain.Entities;

namespace WebAPI.Application.Interfaces.Services
{
	public interface IUserService
	{
		Task<RequestResultDto> CreateUserWithToken(CreateUserDto createUserDto, string roleNam);
		Task<LoginResultDto> LoginUser(LoginUserDto loginUserDto);
		Task<bool> UpdateCurrentUserPassword(string password);
		Task<bool> UpdatCurrentUserName(JsonPatchDocument<UpdateUserNameDto> updateNameDto);
		Task<RequestResultDto> UpdateUserRole(string roleName, int userID);

		Task<ResponseUserDto> GetCurrentUserInformation();
		int GetCurrentUserId();
		Task<User> GetCurrentUserAsync();
		Task<List<string>> GetCurrentUserRolesAsync();
	}
}
