
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebAPI.Domain.Entities;
using WebAPI.Persistence.Context;

namespace TestProject
{
	public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
	{
		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{

			builder.ConfigureServices(services =>
			{
				var serviceDescriptor = services.FirstOrDefault(x => x.ServiceType == typeof(DbContextOptions<DataContext>));

				services.Remove(serviceDescriptor);

				services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("TestDB"));

				var serviceProvider = services.BuildServiceProvider();

				using var scope = serviceProvider.CreateScope();

				var db = scope.ServiceProvider.GetRequiredService<DataContext>();

				db.Database.EnsureCreated();

				db.User.Add(new User { FirstName = $"ad_1", LastName = $"soyad_1"/*, RoleId = 1*/});

				db.SaveChanges();

				for (int i = 0; i < 10; i++)
				{
					db.Account.Add(new Account { Balance = 1000, UserID = 1, AccountNumber = $"000 0000 0000 0000"+i, MinimumBalance = 100 });
				}

				db.SaveChanges();

				
				//db.SaveChanges();



				builder.UseEnvironment("Development");
			});
		}
	}
}
