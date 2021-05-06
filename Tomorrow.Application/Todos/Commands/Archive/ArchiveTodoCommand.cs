using System;
using MediatR;

namespace Tomorrow.Application.Todos.Commands.Archive
{
	public record ArchiveTodoCommand(Guid TodoId) : IRequest;
}