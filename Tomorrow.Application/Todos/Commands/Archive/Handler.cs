using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Tomorrow.DomainModel;
using Tomorrow.DomainModel.Todos;

namespace Tomorrow.Application.Todos.Commands.Archive
{
	internal class Handler : IRequestHandler<ArchiveTodoCommand>
	{
		private readonly IAccountProvider accountProvider;
		private readonly CustomDbContext dbContext;

		public Handler(CustomDbContext dbContext, IAccountProvider accountProvider)
		{
			this.dbContext = dbContext;
			this.accountProvider = accountProvider;
		}

		public async Task<Unit> Handle(ArchiveTodoCommand request, CancellationToken cancellationToken)
		{
			var account = await accountProvider.GetCurrentAsync(cancellationToken);
			var todoId = new Identifier<Todo>(request.TodoId);
			var todo = await dbContext.Todos.FindAsync(new object[] { todoId }, cancellationToken);

			if (todo is null)
				throw new Exception("Todo not found");

			if (todo.OwnerId != account.Id)
				throw new Exception("Not authorized");

			todo.Archive();

			await dbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}