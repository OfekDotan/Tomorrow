using System.Diagnostics.CodeAnalysis;
using Tomorrow.DomainModel.Accounts;
using Tomorrow.DomainModel.Groups;

namespace Tomorrow.DomainModel.Todos
{
	public class Todo : AggregateRoot<Todo>
	{
		public Todo(Identifier<Todo> id, Account account, string name, Priority priority)
			: base(id)
		{
			Name = name;
			Priority = priority;
			OwnerId = account.Id;
		}

		private Todo(Identifier<Todo> id) : base(id)
		{
		}

		[MemberNotNullWhen(true, nameof(GroupId))]
		public bool BelongsToGroup => GroupId is not null;

		public Identifier<Group>? GroupId { get; private set; }
		public string Name { get; private set; }
		public Identifier<Account> OwnerId { get; }
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

		public void Rename(string name)
		{
			Name = name;
		}
	}
}