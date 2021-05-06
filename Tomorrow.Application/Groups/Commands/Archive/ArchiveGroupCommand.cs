using System;
using MediatR;

namespace Tomorrow.Application.Groups.Commands.Delete
{
	public record ArchiveGroupCommand(Guid groupId) : IRequest;
}