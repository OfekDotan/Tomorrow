using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Tomorrow.Application
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
		{
			services.AddMediatR(Assembly.GetExecutingAssembly());
			return services;
		}
	}
}