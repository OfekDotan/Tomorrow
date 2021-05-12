using System;
using Tomorrow.DomainModel.Accounts;

namespace Tomorrow.DomainModel.Groups
{
	public class Group : AggregateRoot<Group>
	{
		public Group(Identifier<Group> id, string name, Account owner) : base(id)
		{
			Name = name;
			OwnerId = owner.Id;
			Archived = false;
		}

		private Group(Identifier<Group> id) : base(id)
		{
		}

		public bool Archived { get; private set; }
		public string Name { get; }

		public Identifier<Account> OwnerId { get; }

		public void Archive()
		{
			if (Archived)
				throw new Exception("Group already archived");
			Archived = true;
		}
	}
}