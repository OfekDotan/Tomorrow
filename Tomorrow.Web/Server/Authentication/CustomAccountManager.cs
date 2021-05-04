using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tomorrow.Application;
using Tomorrow.DomainModel;
using Tomorrow.DomainModel.Accounts;
using Tomorrow.Web.Server.Models;

namespace Tomorrow.Web.Server.Authentication
{
	public class CustomAccountManager : UserManager<ApplicationUser>
	{
		private readonly CustomDbContext customDbContext;

		public CustomAccountManager(IUserStore<ApplicationUser> store,
			IOptions<IdentityOptions> optionsAccessor,
			IPasswordHasher<ApplicationUser> passwordHasher,
			IEnumerable<IUserValidator<ApplicationUser>> userValidators,
			IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
			ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
			IServiceProvider services,
			ILogger<UserManager<ApplicationUser>> logger, CustomDbContext customDbContext)
			: base(store, optionsAccessor, passwordHasher, userValidators,
				  passwordValidators, keyNormalizer, errors, services, logger)
		{
			this.customDbContext = customDbContext;
		}

		public override async Task<IdentityResult> CreateAsync(ApplicationUser user)
		{
			var result = await base.CreateAsync(user);
			if (result.Succeeded)
			{
				var account = new Account(new Identifier<Account>(Guid.Parse(user.Id)));
				await customDbContext.Accounts.AddAsync(account);
				await customDbContext.SaveChangesAsync();
			}
			return result;
		}
	}
}