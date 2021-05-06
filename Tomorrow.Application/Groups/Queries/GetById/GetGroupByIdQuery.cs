using System;
using MediatR;

namespace Tomorrow.Application.Groups.Queries.GetById
{
	public record GetGroupByIdQuery(Guid GroupId) : IRequest<GroupDto>;
}