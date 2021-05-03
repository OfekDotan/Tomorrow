namespace Tomorrow.DomainModel.Groups
{
	public class Group : AggregateRoot<Group>
	{
		public Group(Identifier<Group> id, string name) : base(id)
		{
			Name = name;
		}

		public string Name { get; }
	}
}