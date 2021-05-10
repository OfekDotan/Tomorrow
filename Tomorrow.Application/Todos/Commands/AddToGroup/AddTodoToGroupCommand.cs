using System;
using MediatR;

namespace Tomorrow.Application.Todos.Commands.AddToGroup
{
	public record AddTodoToGroupCommand(Guid TodoId, Guid GroupId) : IRequest;
}