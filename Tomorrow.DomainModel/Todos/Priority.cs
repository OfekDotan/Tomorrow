using System;

namespace Tomorrow.DomainModel.Todos
{
	public record Priority : IComparable<Priority>
	{
		private readonly int priority;

		public Priority(int priority)
		{
			this.priority = priority;
		}

		public int CompareTo(Priority? other)
		{
			if (other is null)
				return 1;

			return priority.CompareTo(other.priority);
		}
	}
}