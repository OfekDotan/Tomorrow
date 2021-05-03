using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tomorrow.Application;
using Tomorrow.Web.Server.Data;

namespace Tomorrow.Web.Server
{
	public class Program
	{
		public static IHostBuilder CreateHostBuilder(string[] args)
		{
			return Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
		}

		public static async Task Main(string[] args)
		{
			var host = CreateHostBuilder(args).Build();

			await MigrateDatebaseAsync(host.Services);

			await host.RunAsync();
		}

		private static async Task MigrateDatebaseAsync(IServiceProvider serviceProvider)
		{
			using var serviceScope = serviceProvider.CreateScope();

			var identityContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			await identityContext.Database.MigrateAsync();

			var mainDbContext = serviceScope.ServiceProvider.GetRequiredService<CustomDbContext>();
			await mainDbContext.Database.MigrateAsync();
		}
	}
}