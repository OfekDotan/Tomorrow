using System.Collections.Generic;
using MediatR;

namespace Tomorrow.Application.Todos.Queries.List
{
	public record ListTodosQuery(int Limit, int Offset) : IRequest<IReadOnlyList<TodoDto>>;
}