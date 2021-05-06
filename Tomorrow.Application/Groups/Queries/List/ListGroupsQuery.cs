using System.Collections.Generic;
using MediatR;

namespace Tomorrow.Application.Groups.Queries.List
{
	public record ListGroupsQuery(int limit, int offset) : IRequest<IReadOnlyList<GroupDto>>;
}