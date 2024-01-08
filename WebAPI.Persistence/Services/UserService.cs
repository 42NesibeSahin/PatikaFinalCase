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

namespace WebAPI.Persistence.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        public UserService(IUserRepository userRepository, IMapper mapper, UserManager<User> userManager, IConfiguration configuration)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        }
        public async Task<int> CreateUser(CreateUserDto createUserDto, int roleID)
        {
           
            User user = _mapper.Map<User>(createUserDto);
            user.RoleId = roleID;
            user.PasswordHash = UserHelper.HashPassword(user, createUserDto.Password);
            await _userRepository.AddAsync(user);
            await _userRepository.Save();
            return user.Id;
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
            }
            return loginResultDto;
        }

        public async Task<bool> UpdateUserPassword(string password, int userID)
        {
            bool result = false;
            var user = await _userManager.FindByIdAsync(userID.ToString());
            if (user != null)
            {
                user.PasswordHash = UserHelper.HashPassword(user, password);
                await _userRepository.UpdateAsync(user);
                await _userRepository.Save();
                result = true;
            }
            return result;
        }

        public async Task<bool> UpdateUserName(JsonPatchDocument<UpdateUserNameDto> updateNameDto, int userID)
        {
            bool result = false;
            var user = await _userManager.FindByIdAsync(userID.ToString());
            if (user != null)
            {
                var userPatch = _mapper.Map<UpdateUserNameDto>(user);

                updateNameDto.ApplyTo(userPatch);

                _mapper.Map(userPatch, user);

                await _userRepository.Save();
                result = true;
            }
            return result;
        }

        public async Task<ResponseUserDto> GetUserInformation(int userID)
        {

            var user = await _userManager.FindByIdAsync(userID.ToString());
            ResponseUserDto responseUserDto = _mapper.Map<ResponseUserDto>(user);

            return responseUserDto;
        }

        public async Task<bool> UpdateUserRole(int roleID, int userID)
        {
            bool result = false;
            var user = await _userManager.FindByIdAsync(userID.ToString());
            if (user != null)
            {
                user.RoleId = roleID;
                await _userRepository.UpdateAsync(user);
                await _userRepository.Save();
                result = true;
            }
            return result;
        }

        private async Task<string> GenerateToken(User user)
        {

            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.FirstName + user.LastName));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.UserName));
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var tokenDesc = new SecurityTokenDescriptor
            {
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
                Expires = DateTime.UtcNow.AddHours(2),
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



