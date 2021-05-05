using System;

namespace Tomorrow.Application.Todos
{
	public record TodoDto(Guid Id, string Name, int Priority);
}