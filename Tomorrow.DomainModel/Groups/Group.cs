namespace Tomorrow.DomainModel.Groups
{
    public class Group : AggregateRoot<Group>
    {
        public Group(Identifier<Group> id) : base(id)
        {
        }
    }
}