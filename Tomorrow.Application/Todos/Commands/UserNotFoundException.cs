﻿using System;
using System.Runtime.Serialization;

namespace Tomorrow.Application.Todos.Commands
{
	[Serializable]
	public class UserNotFoundException : Exception
	{
		public UserNotFoundException()
		{
		}

		public UserNotFoundException(string? message) : base(message)
		{
		}

		public UserNotFoundException(string? message, Exception? innerException) : base(message, innerException)
		{
		}

		protected UserNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}