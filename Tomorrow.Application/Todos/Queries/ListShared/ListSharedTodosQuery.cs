using System.Collections.Generic;
using MediatR;

namespace Tomorrow.Application.Todos.Queries.ListShared
{
	public record ListSharedTodosQuery(int Limit, int Offset) : IRequest<IReadOnlyList<TodoDto>>;
}