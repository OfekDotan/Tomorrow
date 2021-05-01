using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tomorrow.Application.Todos.Commands.Create;

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

		// POST api/<TodosController>
		[HttpPost]
		public async Task<IActionResult> CreateAsync([FromBody] CreateTodoCommand command)
		{
			var todoId = await requestSender.Send(command);
			var todo = new
			{
				Id = todoId,
			};
			return CreatedAtAction("GetById", "Todos", new { Id = todoId }, todo);
		}

		// DELETE api/<TodosController>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}

		// GET: api/<TodosController>
		[HttpGet]
		public IEnumerable<string> Get()
		{
			return new string[] { "value1", "value2" };
		}

		// GET api/<TodosController>/5
		[HttpGet("{id}")]
		public string Get(int id)
		{
			return "value";
		}

		// PUT api/<TodosController>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}
	}
}