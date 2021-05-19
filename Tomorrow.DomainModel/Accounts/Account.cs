using System.Collections.Generic;
using Tomorrow.DomainModel.Todos;

namespace Tomorrow.DomainModel.Accounts
{
	public class Account : AggregateRoot<Account>
	{
		public Account(Identifier<Account> id) : base(id)
		{
		}

		public List<Todo> EditableTodos { get; }
		public List<Todo> ViewableTodos { get; }
	}
}