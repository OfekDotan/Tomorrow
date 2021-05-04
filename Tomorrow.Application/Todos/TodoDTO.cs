using System;

namespace Tomorrow.Application.Todos
{
	public record TodoDTO(Guid Id, string Name, int Priority);
}