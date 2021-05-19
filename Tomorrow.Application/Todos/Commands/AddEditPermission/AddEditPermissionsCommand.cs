using System;
using MediatR;

namespace Tomorrow.Application.Todos.Commands.AddEditPermission
{
	public record AddEditPermissionsCommand(Guid Id, string Email) : IRequest;
}