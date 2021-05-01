using System;

namespace Tomorrow.DomainModel
{
	/// <summary>
	/// A domain concept that can change state over time.
	/// </summary>
	/// <typeparam name="TEntity">The concrete type of the entity.</typeparam>
	public abstract class Entity<TEntity> where TEntity : Entity<TEntity>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Entity{TEntity}"/> class using a unique identifier.
		/// </summary>
		/// <param name="id">The unique identifier.</param>
		protected Entity(Identifier<TEntity> id)
		{
			Id = id;
		}

		/// <summary>
		/// Gets the unique identifier of this <see cref="Entity{TEntity}"/>.
		/// </summary>
		public Identifier<TEntity> Id { get; }

		/// <summary>
		/// Implements the operator != for determining if two Entities does not share the same ID.
		/// </summary>
		/// <param name="left">The first <see cref="Entity{TEntity}"/>.</param>
		/// <param name="right">The second <see cref="Entity{TEntity}"/>.</param>
		/// <returns>
		/// <see langword="true"/> if the instances represent two distinct Entities; <see langword="false"/> otherwise.
		/// </returns>
		public static bool operator !=(Entity<TEntity>? left, Entity<TEntity>? right) => !(left == right);

		/// <summary>
		/// Implements the operator != for determining if two Entities share the same ID.
		/// </summary>
		/// <param name="left">The first <see cref="Entity{TEntity}"/>.</param>
		/// <param name="right">The second <see cref="Entity{TEntity}"/>.</param>
		/// <returns>
		/// <see langword="true"/> if the instances represent the same Entity; <see langword="false"/> otherwise.
		/// </returns>
		public static bool operator ==(Entity<TEntity>? left, Entity<TEntity>? right) => Equals(left, right);

		/// <inheritdoc/>
		public override bool Equals(object? obj)
		{
			return obj is Entity<TEntity> entity && Id == entity.Id;
		}

		/// <inheritdoc/>
		public override int GetHashCode()
		{
			return HashCode.Combine(Id);
		}
	}
}