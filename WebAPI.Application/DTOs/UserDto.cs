using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Application.DTOs
{
    public class UserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }

    public class CreateUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
    public class LoginUserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class LoginResultDto : RequestResultDto
    {
        public string Token { get; set; }
    }

    public class UpdateUserNameDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class ResponseUserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
    }


}
