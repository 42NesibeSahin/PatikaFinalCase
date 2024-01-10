
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebAPI.Domain.Entities;
using WebAPI.Persistence.Context;

namespace TestProject
{
	public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
	{
		//private readonly UserManager<User> _userManager;

		public CustomWebApplicationFactory(UserManager<User> userManager)
        {
			//_userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
		}
        protected override void ConfigureWebHost(IWebHostBuilder builder)
		{

			builder.ConfigureServices(async services =>
			{
				var serviceDescriptor = services.FirstOrDefault(x => x.ServiceType == typeof(DbContextOptions<DataContext>));

				services.Remove(serviceDescriptor);

				services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("TestDB"));

				var serviceProvider = services.BuildServiceProvider();

				using var scope = serviceProvider.CreateScope();

				var db = scope.ServiceProvider.GetRequiredService<DataContext>();

				db.Database.EnsureCreated();

				var testUser = new User()
				{
					UserName = "admin_test",
					Email = "testadmin@gmail.com",
					FirstName = "AdminTest First",
					LastName = "AdminTest Last",
				};
				//await _userManager.CreateAsync(testUser, "T123_t");
				//await _userManager.AddToRoleAsync(testUser, "admin");

				//db.User.Add(new User { FirstName = $"ad_1", LastName = $"soyad_1"/*, RoleId = 1*/});

				//db.SaveChanges();

				for (int i = 0; i < 10; i++)
				{
					db.Account.Add(new Account { Balance = 1000, UserID = testUser.Id, AccountNumber = $"0000 0000 0000 000"+i, MinimumBalance = 100 });
				}

				db.SaveChanges();

				
				//db.SaveChanges();



				builder.UseEnvironment("Development");
			});
		}
	}
}
