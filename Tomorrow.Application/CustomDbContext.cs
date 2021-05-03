using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Tomorrow.DomainModel.Accounts;
using Tomorrow.DomainModel.Groups;
using Tomorrow.DomainModel.Todos;

namespace Tomorrow.Application
{
	public class CustomDbContext : DbContext
	{
		public CustomDbContext(DbContextOptions<CustomDbContext> options) : base(options)
		{
		}

		public DbSet<Account> Accounts { get; init; }
		public DbSet<Group> Groups { get; init; }

		public DbSet<Todo> Todos { get; init; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		}
	}
}