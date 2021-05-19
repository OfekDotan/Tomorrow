using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tomorrow.Application.Todos;
using Tomorrow.Application.Todos.Commands.AddEditPermission;
using Tomorrow.Application.Todos.Commands.AddToGroup;
using Tomorrow.Application.Todos.Commands.AddViewPermission;
using Tomorrow.Application.Todos.Commands.Archive;
using Tomorrow.Application.Todos.Commands.Complete;
using Tomorrow.Application.Todos.Commands.Create;
using Tomorrow.Application.Todos.Commands.Edit;
using Tomorrow.Application.Todos.Queries.GetById;
using Tomorrow.Application.Todos.Queries.ListAll;
using Tomorrow.Application.Todos.Queries.ListByGroup;
using Tomorrow.Application.Todos.Queries.ListWithoutGroup;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tomorrow.Web.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TodosController : ControllerBase
	{
		private readonly ISender requestSender;

		public TodosController(ISender requestSender)
		{
			this.requestSender = requestSender;
		}

		// PUT api/<TodosController>/5/add-viewer
		[HttpPut("{id}/add-viewer")]
		public async Task<IActionResult> AddEditorsAsync(Guid id, [FromBody] AddViewPermissionsCommand command)
		{
			if (id != command.Id)
			{
				throw new Exception("Url id param and request doesn't match");
			}

			await requestSender.Send(command);

			return NoContent();
		}

		// PUT api/<TodosController>/5/add-editor
		[HttpPut("{id}/add-editor")]
		public async Task<IActionResult> AddEditorsAsync(Guid id, [FromBody] AddEditPermissionsCommand command)
		{
			if (id != command.Id)
			{
				throw new Exception("Url id param and request doesn't match");
			}

			await requestSender.Send(command);

			return NoContent();
		}

		// PUT api/<TodosController>/5
		[HttpPut("{id}/add-to-group")]
		public async Task<IActionResult> AddToGroupAsync(Guid id, [FromBody] AddTodoToGroupCommand command)
		{
			if (id != command.TodoId)
			{
				throw new Exception("Url id param and request doesn't match"); // FIXME - Change to custom exception ASAP
			}

			await requestSender.Send(command);

			return NoContent();
		}

		// DELETE api/<TodosController>/5
		[HttpDelete("{id}")]
		public async Task<ActionResult> Archive(Guid id)
		{
			var command = new ArchiveTodoCommand(id);

			await requestSender.Send(command);

			return NoContent();
		}

		// PUT api/<TodosController>/5/complete
		[HttpPut("{id}/complete")]
		public async Task<IActionResult> CompleteAsync(Guid id, [FromBody] CompleteTodoCommand command)
		{
			if (id != command.Id)
			{
				throw new Exception("Url id param and request doesn't match");
			}

			await requestSender.Send(command);

			return NoContent();
		}

		// POST api/<TodosController>
		[HttpPost]
		public async Task<IActionResult> CreateAsync([FromBody] CreateTodoCommand command)
		{
			var todoId = await requestSender.Send(command);
			var todo = new
			{
				Id = todoId,
				command.Name,
				command.Priority,
				command.groupId
			};
			return CreatedAtAction("GetById", "Todos", new { Id = todoId }, todo);
		}

		// GET: api/<TodosController>
		[HttpGet]
		public async Task<ActionResult<IEnumerable<TodoDto>>> GetAsListAsync(int limit, int offset, bool listAll, Guid? groupId)
		{
			IRequest<IReadOnlyList<TodoDto>> query;
			if (groupId.HasValue)
				query = new ListTodosByGroupQuery(groupId.Value, limit, offset);
			else if (listAll)
				query = new ListAllTodosQuery(limit, offset);
			else
				query = new ListTodosWithoutGroupQuery(limit, offset);

			var todos = await requestSender.Send(query);

			return Ok(todos);
		}

		// GET api/<TodosController>/5
		[HttpGet("{id}")]
		public async Task<ActionResult<TodoDto>> GetByIdAsync(Guid id)
		{
			var query = new GetTodoByIdQuery(id);

			var todo = await requestSender.Send(query);

			return Ok(todo);
		}

		// PUT api/<TodosController>/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutAsync(Guid id, [FromBody] EditTodoCommand command)
		{
			if (id != command.Id)
			{
				throw new Exception("Url id param and request doesn't match");
			}

			await requestSender.Send(command);

			return NoContent();
		}
	}
}