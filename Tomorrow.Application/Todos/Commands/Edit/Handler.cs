using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Tomorrow.DomainModel;
using Tomorrow.DomainModel.Groups;
using Tomorrow.DomainModel.Todos;

namespace Tomorrow.Application.Todos.Commands.Edit
{
	internal class Handler : IRequestHandler<EditTodoCommand>
	{
		private readonly IAccountProvider accountProvider;
		private readonly CustomDbContext dbContext;

		public Handler(IAccountProvider accountProvider, CustomDbContext dbContext)
		{
			this.accountProvider = accountProvider;
			this.dbContext = dbContext;
		}

		public async Task<Unit> Handle(EditTodoCommand request, CancellationToken cancellationToken)
		{
			var currentAccount = await accountProvider.GetCurrentAsync(cancellationToken);
			var todoId = new Identifier<Todo>(request.Id);

			if (string.IsNullOrWhiteSpace(request.Name))
				throw new InvalidOperationException("Todo name is required");
			if (request.Name.Length > 256)
				throw new InvalidOperationException("Todo name is too long");

			var todo = await dbContext.Todos.FindAsync(new object[] { todoId }, cancellationToken);
			if (todo is null || todo.OwnerId != currentAccount.Id)
				throw new Exception("Todo not found"); //FIXME - change to custom exception asap

			todo.Rename(request.Name);
			todo.ChangePriority(new Priority(request.Priority));
			if (todo.GroupId is not null && request.GroupId is null)
				todo.RemoveFromGroup();

			if (request.GroupId is not null)
			{
				var groupId = new Identifier<Group>(request.GroupId.Value);
				var group = await dbContext.Groups.FindAsync(new object[] { groupId }, cancellationToken);
				if (group is null || group.OwnerId != currentAccount.Id)
					throw new Exception("Group not found"); //FIXME - change to custom exception asap
				todo.AddTo(group);
			}

			await dbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}