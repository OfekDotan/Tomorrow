using System;
using MediatR;

namespace Tomorrow.Application.Todos.Commands.AddViewPermission
{
	public record AddViewPermissionsCommand(Guid Id, string Email) : IRequest;
}