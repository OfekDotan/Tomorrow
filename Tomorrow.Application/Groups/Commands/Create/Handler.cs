using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Tomorrow.DomainModel;
using Tomorrow.DomainModel.Groups;

namespace Tomorrow.Application.Groups.Commands.Create
{
	internal class Handler : IRequestHandler<CreateGroupCommand, Guid>
	{
		private readonly IAccountProvider accountProvider;
		private readonly CustomDbContext dbContext;

		public Handler(IAccountProvider accountProvider, CustomDbContext dbContext)
		{
			this.accountProvider = accountProvider;
			this.dbContext = dbContext;
		}

		public async Task<Guid> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
		{
			if (string.IsNullOrWhiteSpace(request.Name))
				throw new InvalidOperationException("Todo name is required");
			if (request.Name.Length > 256)
				throw new InvalidOperationException("Todo name is too long");

			var account = await accountProvider.GetCurrentAsync(cancellationToken);
			var id = new Identifier<Group>(Guid.NewGuid());
			var group = new Group(id, request.Name, account);

			dbContext.Groups.Add(group);
			await dbContext.SaveChangesAsync(cancellationToken);

			return group.Id.ToGuid();
		}
	}
}