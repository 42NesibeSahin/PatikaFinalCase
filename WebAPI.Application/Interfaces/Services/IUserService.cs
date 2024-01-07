using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Application.DTOs;

namespace WebAPI.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<int> CreateUser(CreateUserDto createUserDto, int roleID);
        Task<LoginResultDto> LoginUser(LoginUserDto loginUserDto);
        Task<bool> UpdateUserPassword(string password, int userID);
        Task<bool> UpdateUserName(JsonPatchDocument<UpdateUserNameDto> updateNameDto, int userID);
        Task<bool> UpdateUserRole(int roleID, int userID);

        Task<ResponseUserDto> GetUserInformation(int userID);
    }
}
