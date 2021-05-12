using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tomorrow.Application.Groups.Commands.Delete;
using Tomorrow.DomainModel;
using Tomorrow.DomainModel.Groups;

namespace Tomorrow.Application.Groups.Commands.Archive
{
	internal class Handler : IRequestHandler<ArchiveGroupCommand>
	{
		private readonly IAccountProvider accountProvider;
		private readonly CustomDbContext dbContext;

		public Handler(CustomDbContext dbContext, IAccountProvider accountProvider)
		{
			this.dbContext = dbContext;
			this.accountProvider = accountProvider;
		}

		public async Task<Unit> Handle(ArchiveGroupCommand request, CancellationToken cancellationToken)
		{
			var groupId = new Identifier<Group>(request.groupId);
			var account = await accountProvider.GetCurrentAsync(cancellationToken);
			var group = await dbContext.Groups.FindAsync(new object[] { groupId }, cancellationToken);

			if (group is null)
				throw new Exception("Group not found");

			if (group.OwnerId != account.Id)
				throw new Exception("Not authorized");
			var todosInGroup = await dbContext.Todos
				.Where(todo => todo.GroupId == group.Id)
				.Where(todo => !todo.Archived)
				.ToListAsync(cancellationToken);

			foreach (var todo in todosInGroup)
				todo.Archive();

			group.Archive();

			await dbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}