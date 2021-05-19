using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Tomorrow.Application.Todos.Queries.ListWithoutGroup
{
	internal class Handler : IRequestHandler<ListTodosWithoutGroupQuery, IReadOnlyList<TodoDto>>
	{
		private readonly IAccountProvider accountProvider;
		private readonly CustomDbContext customDbContext;

		public Handler(IAccountProvider accountProvider, CustomDbContext customDbContext)
		{
			this.accountProvider = accountProvider;
			this.customDbContext = customDbContext;
		}

		public async Task<IReadOnlyList<TodoDto>> Handle(ListTodosWithoutGroupQuery request, CancellationToken cancellationToken)
		{
			var currentAccount = await accountProvider.GetCurrentAsync(cancellationToken);

			var todos = await customDbContext.Todos
					.AsNoTracking()
					.Where(todo => !todo.Archived)
					.Where(todo => todo.GroupId == null)
					.OrderBy(t => t.Completed ? 1 : 0)
					.ThenByDescending(t => EF.Property<int>(t.Priority, "priority"))
					.Include(t => t.accountsThatCanEdit)
					.Include(t => t.accountsThatCanView)
					.ToListAsync(cancellationToken);

			var todoDtos = todos
			.Where(t => t.CanView(currentAccount))
			.Skip(request.Offset)
			.Take(request.Limit)
			.Select(todo => new TodoDto(todo.Id.ToGuid(), todo.Name, todo.Priority.ToInt32(), todo.Completed, todo.GroupId, false))
			.ToList();

			return todoDtos.AsReadOnly();
		}
	}
}