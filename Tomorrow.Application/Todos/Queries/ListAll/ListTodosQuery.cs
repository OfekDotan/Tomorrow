using System.Collections.Generic;
using MediatR;

namespace Tomorrow.Application.Todos.Queries.ListAll
{
	public record ListAllTodosQuery(int Limit, int Offset) : IRequest<IReadOnlyList<TodoDto>>;
}