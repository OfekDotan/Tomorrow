using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Tomorrow.DomainModel;
using Tomorrow.DomainModel.Todos;

namespace Tomorrow.Application.Todos.Commands.Create
{
	internal class Handler : IRequestHandler<CreateTodoCommand, Guid>
	{
		private readonly IAccountProvider accountProvider;
		private readonly CustomDbContext dbContext;

		public Handler(IAccountProvider accountProvider, CustomDbContext dbContext)
		{
			this.accountProvider = accountProvider;
			this.dbContext = dbContext;
		}

		public async Task<Guid> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
		{
			var currentAccount = await accountProvider.GetCurrentAsync(cancellationToken);
			var todoId = Identifier<Todo>.GenerateNew();

			if (string.IsNullOrWhiteSpace(request.Name))
				throw new InvalidOperationException("Todo name is required");
			if (request.Name.Length > 256)
				throw new InvalidOperationException("Todo name is too long");

			var todo = new Todo(todoId, currentAccount, request.Name, new Priority(request.Priority));
			dbContext.Todos.Add(todo);
			await dbContext.SaveChangesAsync(cancellationToken);

			return todo.Id.ToGuid();
		}
	}
}