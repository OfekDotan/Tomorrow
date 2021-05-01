using System;

namespace Tomorrow.Application
{
	public class UnauthorizedRequestException : Exception
	{
		public UnauthorizedRequestException(string? message) : base(message)
		{
		}
	}
}