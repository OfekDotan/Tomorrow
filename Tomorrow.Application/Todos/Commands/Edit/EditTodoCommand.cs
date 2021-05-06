using System;
using MediatR;

namespace Tomorrow.Application.Todos.Commands.Edit
{
	public record EditTodoCommand(Guid Id, string Name, int Priority) : IRequest;
}