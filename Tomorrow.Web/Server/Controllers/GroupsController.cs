using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tomorrow.Application.Groups.Commands.Create;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tomorrow.Web.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class GroupsController : ControllerBase
	{
		private readonly ISender requestSender;

		public GroupsController(ISender requestSender)
		{
			this.requestSender = requestSender;
		}

		// POST api/<GroupsController>
		[HttpPost]
		public async Task<IActionResult> CreateAsync([FromBody] CreateGroupCommand createGroupCommand)
		{
			var groupId = await requestSender.Send(createGroupCommand);
			var group = new { Id = groupId, createGroupCommand.Name };
			return CreatedAtAction("GetById", "Groups", new { Id = groupId }, group);
		}

		// DELETE api/<GroupsController>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}

		// GET: api/<GroupsController>
		[HttpGet]
		public IEnumerable<string> Get()
		{
			return new string[] { "value1", "value2" };
		}

		// GET api/<GroupsController>/5
		[HttpGet("{id}")]
		public string GetById(int id)
		{
			return "value";
		}

		// PUT api/<GroupsController>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}
	}
}