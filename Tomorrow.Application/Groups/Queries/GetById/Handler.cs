using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Tomorrow.DomainModel;
using Tomorrow.DomainModel.Groups;

namespace Tomorrow.Application.Groups.Queries.GetById
{
	internal class Handler : IRequestHandler<GetGroupByIdQuery, GroupDto>
	{
		private readonly IAccountProvider accountProvider;
		private readonly CustomDbContext dbContext;

		public Handler(CustomDbContext dbContext, IAccountProvider accountProvider)
		{
			this.dbContext = dbContext;
			this.accountProvider = accountProvider;
		}

		public async Task<GroupDto> Handle(GetGroupByIdQuery request, CancellationToken cancellationToken)
		{
			var id = new Identifier<Group>(request.GroupId);
			var group = await dbContext.Groups.FindAsync(new object[] { id }, cancellationToken);
			if (group is null || group.Archived)
				throw new Exception("Group not found");
			var account = await accountProvider.GetCurrentAsync(cancellationToken);
			if (!account.Id.Equals(id))
				throw new Exception("Not authorized to view this group");

			return new GroupDto(id.ToGuid(), group.Name);
		}
	}
}