namespace Tomorrow.DomainModel.Accounts
{
    public class Account : AggregateRoot<Account>
    {
        public Account(Identifier<Account> id) : base(id)
        {
        }
    }
}