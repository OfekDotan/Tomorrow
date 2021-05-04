using System;
using MediatR;

namespace Tomorrow.Application.Groups.Commands.Create
{
	public record CreateGroupCommand(string Name) : IRequest<Guid>;
}