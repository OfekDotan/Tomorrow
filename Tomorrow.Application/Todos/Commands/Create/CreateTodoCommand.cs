using System;
using MediatR;

namespace Tomorrow.Application.Todos.Commands.Create
{
	public record CreateTodoCommand(string Name, int Priority) : IRequest<Guid>;
}