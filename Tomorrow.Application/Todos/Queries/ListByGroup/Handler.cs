using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tomorrow.DomainModel;
using Tomorrow.DomainModel.Groups;

namespace Tomorrow.Application.Todos.Queries.ListByGroup
{
	internal class Handler : IRequestHandler<ListTodosByGroupQuery, IReadOnlyList<TodoDto>>
	{
		private readonly IAccountProvider accountProvider;
		private readonly CustomDbContext customDbContext;

		public Handler(IAccountProvider accountProvider, CustomDbContext customDbContext)
		{
			this.accountProvider = accountProvider;
			this.customDbContext = customDbContext;
		}

		public async Task<IReadOnlyList<TodoDto>> Handle(ListTodosByGroupQuery request, CancellationToken cancellationToken)
		{
			var currentAccount = await accountProvider.GetCurrentAsync(cancellationToken);
			var groupId = new Identifier<Group>(request.groupId);
			var todos = await customDbContext.Todos
					.AsNoTracking()
					.Where(todo => todo.OwnerId == currentAccount.Id)
					.Where(todo => todo.GroupId == groupId)
					.Where(todo => !todo.Archived)
					.OrderBy(t => t.Completed ? 1 : 0)
					.ThenByDescending(t => EF.Property<int>(t.Priority, "priority"))
					.Skip(request.Offset)
					.Take(request.Limit)
					.Select(todo => new TodoDto(todo.Id.ToGuid(), todo.Name, todo.Priority.ToInt32(), todo.Completed, todo.GroupId))
					.ToListAsync(cancellationToken);

			return todos.AsReadOnly();
		}
	}
}