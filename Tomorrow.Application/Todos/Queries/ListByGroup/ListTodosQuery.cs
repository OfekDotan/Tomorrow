using System;
using System.Collections.Generic;
using MediatR;

namespace Tomorrow.Application.Todos.Queries.ListByGroup
{
	public record ListTodosByGroupQuery(Guid groupId, int Limit, int Offset) : IRequest<IReadOnlyList<TodoDto>>;
}