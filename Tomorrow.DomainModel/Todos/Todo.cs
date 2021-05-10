using System;
using System.Diagnostics.CodeAnalysis;
using Tomorrow.DomainModel.Accounts;
using Tomorrow.DomainModel.Groups;

namespace Tomorrow.DomainModel.Todos
{
	public class Todo : AggregateRoot<Todo>
	{
		public Todo(Identifier<Todo> id, Account account, string name, Priority priority, Identifier<Group>? groupId)
			: base(id)
		{
			Name = name;
			Priority = priority;
			OwnerId = account.Id;
			Archived = false;
			Completed = false;
			GroupId = groupId;
		}

		private Todo(Identifier<Todo> id) : base(id)
		{
		}

		public bool Archived { get; private set; }

		[MemberNotNullWhen(true, nameof(GroupId))]
		public bool BelongsToGroup => GroupId is not null;

		public bool Completed { get; set; }

		public Identifier<Group>? GroupId { get; private set; }

		public string Name { get; private set; }

		public Identifier<Account> OwnerId { get; }

		public Priority Priority { get; private set; }

		[MemberNotNull(nameof(GroupId))]
		public void AddTo(Group group)
		{
			GroupId = group.Id;
		}

		public void Archive()
		{
			if (Archived)
				throw new Exception("Todo already archived");
			Archived = true;
		}

		public void ChangePriority(Priority priority)
		{
			Priority = priority;
		}

		public void Complete()
		{
			if (Completed)
				throw new Exception("Todo already completed");
			Completed = true;
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