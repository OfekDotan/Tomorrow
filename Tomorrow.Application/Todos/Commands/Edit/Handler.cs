using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Tomorrow.DomainModel;
using Tomorrow.DomainModel.Todos;

namespace Tomorrow.Application.Todos.Commands.Edit
{
	internal class Handler : IRequestHandler<EditTodoCommand>
	{
		private readonly IAccountProvider accountProvider;
		private readonly CustomDbContext dbContext;

		public Handler(IAccountProvider accountProvider, CustomDbContext dbContext)
		{
			this.accountProvider = accountProvider;
			this.dbContext = dbContext;
		}

		public async Task<Unit> Handle(EditTodoCommand request, CancellationToken cancellationToken)
		{
			var currentAccount = await accountProvider.GetCurrentAsync(cancellationToken);
			var todoId = new Identifier<Todo>(request.Id);

			if (string.IsNullOrWhiteSpace(request.Name))
				throw new InvalidOperationException("Todo name is required");
			if (request.Name.Length > 256)
				throw new InvalidOperationException("Todo name is too long");

			var todo = await dbContext.Todos.FindAsync(new object[] { todoId }, cancellationToken);
			if (todo is null)
				throw new Exception("Todo not found");

			todo.Rename(request.Name);
			todo.ChangePriority(new Priority(request.Priority));

			await dbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}