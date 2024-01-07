
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Runtime.InteropServices;
using Xunit;
using WebAPI.Application.DTOs;

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

			var responseApi = await client.GetAsync("api/account/1");

			responseApi.EnsureSuccessStatusCode();

			var accountContent = await responseApi.Content.ReadAsStringAsync();

			var accountDto = System.Text.Json.JsonSerializer.Deserialize<AccountDto>(accountContent);

			Assert.NotNull(accountDto);

			Assert.Equal(1, accountDto.ID);
		}
	}
}