using System;
using MediatR;

namespace Tomorrow.Application.Todos.Commands.Complete
{
	public record CompleteTodoCommand(Guid Id) : IRequest;
}