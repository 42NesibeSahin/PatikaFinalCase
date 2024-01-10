
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Runtime.InteropServices;
using Xunit;
using WebAPI.Application.DTOs;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace TestProject
{
	public class AccountControllerTest : IClassFixture<CustomWebApplicationFactory<Program>>
	{

		private readonly CustomWebApplicationFactory<Program> _factory;

		public AccountControllerTest(CustomWebApplicationFactory<Program> factory)
		{
			_factory = factory;
		}

		[Fact]
		public async Task GetByAccountId_Returns_Ok()
		{

			var client = _factory.CreateClient();

			LoginUserDto loginUserDto = new LoginUserDto()
			{
				Username = "admin_test",
				Password = "T123_t"
			};
			var content = JsonContent.Create(loginUserDto);

			var response = await client.PostAsync("api/user/login", content);
			response.EnsureSuccessStatusCode(); 
			var responseString = await response.Content.ReadAsStringAsync();
			var loginToken = JsonConvert.DeserializeObject<string>(responseString);



			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginToken);
			var responseApi = await client.GetAsync("api/account/1");
			responseApi.EnsureSuccessStatusCode();

			var responseStringApi = await response.Content.ReadAsStringAsync();
			var accountDto = JsonConvert.DeserializeObject<AccountDto>(responseString);

			Assert.NotNull(accountDto);

			Assert.Equal(1, accountDto.ID);
		}
	}
}