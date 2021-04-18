using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tomorrow.DomainModel.Groups;

namespace Tomorrow.DomainModel.Todos
{
    public interface ITodoRepository : IRepository<Todo>
    {
        Task<IReadOnlyCollection<Todo>> GetTodosByGroupIdAsync(Identifier<Group> groupId, int limit, int offset, CancellationToken cancellationToken = default);
    }
}