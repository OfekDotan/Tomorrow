using System;
using MediatR;

namespace Tomorrow.Application.Todos.Commands.Rename
{
	public record RenameTodoCommand(Guid TodoId, string Name) : IRequest;
}