using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Tomorrow.Application.Groups.Queries.List
{
	internal class Handler : IRequestHandler<ListGroupsQuery, IReadOnlyList<GroupDto>>
	{
		private readonly IAccountProvider accountProvider;
		private readonly CustomDbContext dbContext;

		public Handler(CustomDbContext dbContext, IAccountProvider accountProvider)
		{
			this.dbContext = dbContext;
			this.accountProvider = accountProvider;
		}

		public async Task<IReadOnlyList<GroupDto>> Handle(ListGroupsQuery request, CancellationToken cancellationToken)
		{
			var account = await accountProvider.GetCurrentAsync(cancellationToken);
			var groups = await dbContext.Groups
				.AsNoTracking()
				.Where(g => g.OwnerId == account.Id)
				.Where(g => !g.Archived)
				.OrderBy(g => g.Name)
				.Skip(request.offset)
				.Take(request.limit)
				.Select(g => new GroupDto(g.Id.ToGuid(), g.Name))
				.ToListAsync(cancellationToken);

			return groups.AsReadOnly();
		}
	}
}