using Tomorrow.DomainModel.Accounts;

namespace Tomorrow.DomainModel.Groups
{
	public class Group : AggregateRoot<Group>
	{
		public Group(Identifier<Group> id, string name, Account owner) : base(id)
		{
			Name = name;
			OwnerId = owner.Id;
		}

		private Group(Identifier<Group> id) : base(id)
		{
		}

		public string Name { get; }
		public Identifier<Account> OwnerId { get; }
	}
}