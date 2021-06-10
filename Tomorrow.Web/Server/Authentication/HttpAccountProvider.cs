using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Tomorrow.Application;
using Tomorrow.Application.Todos.Commands;
using Tomorrow.DomainModel;
using Tomorrow.DomainModel.Accounts;
using Tomorrow.Web.Server.Data;

namespace Tomorrow.Web.Server.Authentication
{
	public class HttpAccountProvider : IAccountProvider
	{
		private readonly ApplicationDbContext applicationDbContext;
		private readonly CustomDbContext dbContext;
		private readonly IHttpContextAccessor httpContextAccessor;

		public HttpAccountProvider(IHttpContextAccessor httpContextAccessor, CustomDbContext dbContext, ApplicationDbContext applicationDbContext)
		{
			this.httpContextAccessor = httpContextAccessor;
			this.dbContext = dbContext;
			this.applicationDbContext = applicationDbContext;
		}

		public async Task<Account> GetAccountFromEmailAsync(string email, CancellationToken cancellationToken = default)
		{
			var user = await applicationDbContext.Users.Where(u => u.Email == email).FirstOrDefaultAsync(cancellationToken);
			if (user == default)
				throw new UserNotFoundException("This email doesn't belong to a known user");

			var currentUserId = new Identifier<Account>(Guid.Parse(user.Id));
			var currentAccount = await dbContext.Accounts.FindAsync(new object[] { currentUserId }, cancellationToken);
			if (currentAccount == default)
				throw new UserNotFoundException("This email doesn't belong to a known user");
			return currentAccount;
		}

		public async Task<Account> GetCurrentAsync(CancellationToken cancellationToken = default)
		{
			var httpContext = httpContextAccessor.HttpContext;
			if (httpContext is null)
				throw new NotAuthenticatedException("Account not found");

			if (!httpContext.User.IsAuthenticated())
				throw new NotAuthenticatedException();

			var textUserId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			var currentUserId = new Identifier<Account>(Guid.Parse(textUserId));
			var currentAccount = await dbContext.Accounts.FindAsync(new object[] { currentUserId }, cancellationToken);
			if (currentAccount is null)
				throw new NotAuthenticatedException("Account not found");
			return currentAccount;
		}
	}
}