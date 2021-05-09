using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Tomorrow.DomainModel;
using Tomorrow.DomainModel.Todos;

namespace Tomorrow.Application.Todos.Commands.Complete
{
	internal class Handler : IRequestHandler<CompleteTodoCommand>
	{
		private readonly IAccountProvider accountProvider;
		private readonly CustomDbContext dbContext;

		public Handler(IAccountProvider accountProvider, CustomDbContext dbContext)
		{
			this.accountProvider = accountProvider;
			this.dbContext = dbContext;
		}

		public async Task<Unit> Handle(CompleteTodoCommand request, CancellationToken cancellationToken)
		{
			var currentAccount = await accountProvider.GetCurrentAsync(cancellationToken);
			var todoId = new Identifier<Todo>(request.Id);

			var todo = await dbContext.Todos.FindAsync(new object[] { todoId }, cancellationToken);
			if (todo is null || todo.OwnerId != currentAccount.Id)
				throw new Exception("Todo not found");

			todo.Complete();

			await dbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}