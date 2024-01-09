using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WebAPI.Application.Interfaces.Repositories;
using WebAPI.Application.Interfaces.Services;


namespace WebAPI.Application
{
	public static class ServiceExtApplication
	{
		public static void AddApplicationSubService(this IServiceCollection service)
		{
			service.AddAutoMapper(Assembly.GetExecutingAssembly());		
		}

	}
}
