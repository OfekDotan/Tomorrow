using Microsoft.EntityFrameworkCore;
using Tomorrow.DomainModel.Accounts;
using Tomorrow.DomainModel.Todos;

namespace Tomorrow.Application
{
	public class CustomDbContext : DbContext
	{
		public CustomDbContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<Account> Accounts { get; init; }
		public DbSet<Todo> Todos { get; init; }
	}
}