using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Tomorrow.DomainModel;
using Tomorrow.DomainModel.Todos;

namespace Tomorrow.Application.Todos.Commands.Rename
{
	internal class Handler : IRequestHandler<RenameTodoCommand>
    {
        private readonly IAccountProvider accountProvider;
        private readonly CustomDbContext dbContext;

        public Handler(IAccountProvider accountProvider, CustomDbContext dbContext)
        {
            this.accountProvider = accountProvider;
            this.dbContext = dbContext;
        }

        public async Task<Unit> Handle(RenameTodoCommand request, CancellationToken cancellationToken)
        {
            var todo = await GetTodoAsync(request, cancellationToken);

            await CheckAuthorizationAsync(todo, cancellationToken);
            ValidateRequest(request);

            todo.Rename(request.Name);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private static void ValidateRequest(RenameTodoCommand request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new InvalidOperationException("Todo name is required");
            if (request.Name.Length > 256)
                throw new InvalidOperationException("Todo name is too long");
        }

        private async Task CheckAuthorizationAsync(Todo todo, CancellationToken cancellationToken)
        {
            var currentAccount = await accountProvider.GetCurrentAsync(cancellationToken);

            if (todo.OwnerId != currentAccount.Id)
                throw new UnauthorizedRequestException("Todo can only be renamed by its owner");
        }

        private async Task<Todo> GetTodoAsync(RenameTodoCommand request, CancellationToken cancellationToken)
        {
            var todoId = new Identifier<Todo>(request.TodoId);
            var todo = await dbContext.Todos.FindAsync(new object[] { todoId }, cancellationToken);
            if (todo is null)
                throw new KeyNotFoundException("Todo not found");
            return todo;
        }
    }
}