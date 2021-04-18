using System.Threading;
using System.Threading.Tasks;

namespace Tomorrow.DomainModel
{
    /// <summary>
    /// A subscriber to events of a certain type.
    /// </summary>
    /// <typeparam name="TEvent">The type of events this object can handle.</typeparam>
    public interface IDomainEventHandler<TEvent> where TEvent : DomainEvent
    {
        /// <summary>
        /// Handles a event that occurred in the domain.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">A token used to cancel the operation.</param>
        Task HandleAsync(TEvent domainEvent, CancellationToken cancellationToken);
    }
}