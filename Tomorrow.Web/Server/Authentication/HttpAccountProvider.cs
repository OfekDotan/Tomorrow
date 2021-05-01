using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Http;
using Tomorrow.Application;
using Tomorrow.DomainModel;
using Tomorrow.DomainModel.Accounts;

namespace Tomorrow.Web.Server.Authentication
{
	public class HttpAccountProvider : IAccountProvider
	{
		private readonly CustomDbContext dbContext;
		private readonly IHttpContextAccessor httpContextAccessor;

		public HttpAccountProvider(IHttpContextAccessor httpContextAccessor, CustomDbContext dbContext)
		{
			this.httpContextAccessor = httpContextAccessor;
			this.dbContext = dbContext;
		}

		public async Task<Account> GetCurrentAsync(CancellationToken cancellationToken = default)
		{
			var httpContext = httpContextAccessor.HttpContext;
			if (httpContext is null)
				throw new Exception("There is no current HttpContext");

			if (!httpContext.User.IsAuthenticated())
				throw new NotAuthenticatedException();

			var textUserId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			var currentUserId = new Identifier<Account>(Guid.Parse(textUserId));
			var currentAccount = await dbContext.Accounts.FindAsync(new object[] { currentUserId }, cancellationToken);
			if (currentAccount is null)
				throw new Exception("Account not found");
			return currentAccount;
		}
	}
}