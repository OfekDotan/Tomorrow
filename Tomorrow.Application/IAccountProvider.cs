using System.Threading;
using System.Threading.Tasks;
using Tomorrow.DomainModel.Accounts;

namespace Tomorrow.Application
{
	public interface IAccountProvider
	{
		Task<Account> GetAccountFromEmailAsync(string email, CancellationToken cancellationToken = default);

		Task<Account> GetCurrentAsync(CancellationToken cancellationToken = default);
	}
}