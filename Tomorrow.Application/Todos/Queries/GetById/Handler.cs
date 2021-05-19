using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tomorrow.DomainModel;
using Tomorrow.DomainModel.Todos;

namespace Tomorrow.Application.Todos.Queries.GetById
{
	internal class Handler : IRequestHandler<GetTodoByIdQuery, TodoDto>
	{
		private readonly IAccountProvider accountProvider;
		private readonly CustomDbContext customDbContext;

		public Handler(CustomDbContext customDbContext, IAccountProvider accountProvider)
		{
			this.customDbContext = customDbContext;
			this.accountProvider = accountProvider;
		}

		public async Task<TodoDto> Handle(GetTodoByIdQuery request, CancellationToken cancellationToken)
		{
			var id = new Identifier<Todo>(request.TodoId);
			var todo = await customDbContext.Todos
				.Include(t => t.accountsThatCanEdit)
				.Include(t => t.accountsThatCanView)
				.SingleOrDefaultAsync(t => t.Id == id, cancellationToken);

			if (todo is null || todo.Archived)
				throw new Exception("Todo not found");
			var currentAccount = await accountProvider.GetCurrentAsync(cancellationToken);
			if (!todo.CanView(currentAccount))
				throw new Exception("Todo not found");

			var groupId = todo.GroupId?.ToGuid();
			return new TodoDto(todo.Id.ToGuid(), todo.Name, todo.Priority.ToInt32(), todo.Completed, groupId, false);
		}
	}
}