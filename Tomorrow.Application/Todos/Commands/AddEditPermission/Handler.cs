using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tomorrow.DomainModel;
using Tomorrow.DomainModel.Todos;

namespace Tomorrow.Application.Todos.Commands.AddEditPermission
{
	internal class Handler : IRequestHandler<AddEditPermissionsCommand>
	{
		private readonly IAccountProvider accountProvider;
		private readonly CustomDbContext dbContext;

		public Handler(IAccountProvider accountProvider, CustomDbContext dbContext)
		{
			this.accountProvider = accountProvider;
			this.dbContext = dbContext;
		}

		public async Task<Unit> Handle(AddEditPermissionsCommand request, CancellationToken cancellationToken)
		{
			var currentAccount = await accountProvider.GetCurrentAsync(cancellationToken);
			var todoId = new Identifier<Todo>(request.Id);

			var todo = await dbContext.Todos
				.Include(t => t.accountsThatCanEdit)
				.Include(t => t.accountsThatCanView)
				.SingleOrDefaultAsync(t => t.Id == todoId, cancellationToken);

			if (todo is null || !todo.CanEdit(currentAccount))
				throw new Exception("Todo not found");

			var account = await accountProvider.GetAccountFromEmailAsync(request.Email, cancellationToken);
			if (account.Id == currentAccount.Id)
				throw new Exception("You can't add your account");

			todo.accountsThatCanEdit.Add(account);
			await dbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}