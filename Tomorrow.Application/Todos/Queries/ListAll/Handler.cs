using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Tomorrow.Application.Todos.Queries.ListAll
{
	internal class Handler : IRequestHandler<ListAllTodosQuery, IReadOnlyList<TodoDto>>
	{
		private readonly IAccountProvider accountProvider;
		private readonly CustomDbContext customDbContext;

		public Handler(IAccountProvider accountProvider, CustomDbContext customDbContext)
		{
			this.accountProvider = accountProvider;
			this.customDbContext = customDbContext;
		}

		public async Task<IReadOnlyList<TodoDto>> Handle(ListAllTodosQuery request, CancellationToken cancellationToken)
		{
			var currentAccount = await accountProvider.GetCurrentAsync(cancellationToken);

			var todos = await customDbContext.Todos
					.AsNoTracking()
					.Where(todo => todo.OwnerId == currentAccount.Id)
					.Where(todo => !todo.Archived)
					.OrderByDescending(t => EF.Property<int>(t.Priority, "priority"))
					.Skip(request.Offset)
					.Take(request.Limit)
					.Select(todo => new TodoDto(todo.Id.ToGuid(), todo.Name, todo.Priority.ToInt32()))
					.ToListAsync(cancellationToken);

			return todos.AsReadOnly();
		}
	}
}