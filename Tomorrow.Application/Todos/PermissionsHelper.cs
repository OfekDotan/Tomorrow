using Tomorrow.DomainModel.Accounts;
using Tomorrow.DomainModel.Todos;

namespace Tomorrow.Application.Todos
{
	internal static class PermissionsHelper
	{
		public static bool CanEdit(this Todo todo, Account account)
		{
			var todoOwnerId = todo.OwnerId;
			return account.Id == todoOwnerId || todo.accountsThatCanEdit.Contains(account);
		}

		public static bool CanView(this Todo todo, Account account)
		{
			var todoOwnerId = todo.OwnerId;
			return account.Id == todoOwnerId || todo.accountsThatCanView.Contains(account) || todo.accountsThatCanEdit.Contains(account);
		}
	}
}