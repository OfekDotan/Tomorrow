using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Tomorrow.DomainModel;
using Tomorrow.DomainModel.Groups;
using Tomorrow.DomainModel.Todos;

namespace Tomorrow.Application.Todos.Commands.AddToGroup
{
	internal class Handler : IRequestHandler<AddTodoToGroupCommand>
	{
		private readonly IAccountProvider accountProvider;
		private readonly CustomDbContext dbContext;

		public Handler(IAccountProvider accountProvider, CustomDbContext dbContext)
		{
			this.accountProvider = accountProvider;
			this.dbContext = dbContext;
		}

		public async Task<Unit> Handle(AddTodoToGroupCommand request, CancellationToken cancellationToken)
		{
			var currentAccount = await accountProvider.GetCurrentAsync(cancellationToken);
			var todoId = new Identifier<Todo>(request.TodoId);
			var groupId = new Identifier<Group>(request.GroupId);

			var todo = await dbContext.Todos.FindAsync(new object[] { todoId }, cancellationToken);
			if (todo is null || todo.OwnerId != currentAccount.Id)
				throw new Exception("Todo not found");

			var group = await dbContext.Groups.FindAsync(new object[] { groupId }, cancellationToken);
			if (group is null || group.OwnerId != currentAccount.Id)
				throw new Exception("Group not found");

			todo.AddTo(group);

			await dbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}