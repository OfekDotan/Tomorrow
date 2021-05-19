using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
			var todo = await dbContext.Todos
				.Include(t => t.accountsThatCanEdit)
				.Include(t => t.accountsThatCanView)
				.SingleOrDefaultAsync(t => t.Id == todoId, cancellationToken);

			if (todo is null)
				throw new Exception("Todo not found");

			if (!todo.CanEdit(account))
				throw new Exception("Not authorized");

			todo.Archive();

			await dbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}