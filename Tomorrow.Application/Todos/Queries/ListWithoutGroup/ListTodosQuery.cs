using System.Collections.Generic;
using MediatR;

namespace Tomorrow.Application.Todos.Queries.ListWithoutGroup
{
	public record ListTodosWithoutGroupQuery(int Limit, int Offset) : IRequest<IReadOnlyList<TodoDto>>;
}