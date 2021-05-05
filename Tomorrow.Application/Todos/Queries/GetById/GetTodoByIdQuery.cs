using System;
using MediatR;

namespace Tomorrow.Application.Todos.Queries.GetById
{
	public record GetTodoByIdQuery(Guid TodoId) : IRequest<TodoDto>;
}