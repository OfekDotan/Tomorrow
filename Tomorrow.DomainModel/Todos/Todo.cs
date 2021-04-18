using System.Diagnostics.CodeAnalysis;
using Tomorrow.DomainModel.Groups;

namespace Tomorrow.DomainModel.Todos
{
    public class Todo : AggregateRoot<Todo>
    {
        public Todo(Identifier<Todo> id, Priority priority)
            : base(id)
        {
            Priority = priority;
        }

        [MemberNotNullWhen(true, nameof(GroupId))]
        public bool BelongsToGroup => GroupId is not null;

        public Identifier<Group>? GroupId { get; private set; }
        public Priority Priority { get; private set; }

        [MemberNotNull(nameof(GroupId))]
        public void AddTo(Group group)
        {
            GroupId = group.Id;
        }

        public void ChangePriority(Priority priority)
        {
            Priority = priority;
        }

        public void RemoveFromGroup()
        {
            GroupId = null;
        }
    }
}