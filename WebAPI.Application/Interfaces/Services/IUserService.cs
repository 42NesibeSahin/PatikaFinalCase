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
		Task<RequestResultDto> CreateUser(CreateUserDto createUserDto, string roleNam);
		Task<LoginResultDto> LoginUser(LoginUserDto loginUserDto);
		Task<bool> UpdateUserPassword(string password);
		Task<bool> UpdateUserName(JsonPatchDocument<UpdateUserNameDto> updateNameDto, int userID);
		Task<bool> UpdateUserRole(int roleID, int userID);

		Task<ResponseUserDto> GetUserInformation(int userID);
		int GetUserId();
		Task<User> GetUserAsync();
		Task<List<string>> GetCurrentUserRolesAsync();
	}
}
