using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tomorrow.Application.Groups.Commands.Create;
using Tomorrow.Application.Groups.Commands.Delete;
using Tomorrow.Application.Groups.Queries;
using Tomorrow.Application.Groups.Queries.GetById;
using Tomorrow.Application.Groups.Queries.List;

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

		// DELETE api/<GroupsController>/5
		[HttpDelete("{id}")]
		public async Task<ActionResult> Archive(Guid id)
		{
			var command = new ArchiveGroupCommand(id);

			await requestSender.Send(command);

			return NoContent();
		}

		// POST api/<GroupsController>
		[HttpPost]
		public async Task<IActionResult> CreateAsync([FromBody] CreateGroupCommand createGroupCommand)
		{
			var groupId = await requestSender.Send(createGroupCommand);
			var group = new { Id = groupId, createGroupCommand.Name };
			return CreatedAtAction("GetById", "Groups", new { Id = groupId }, group);
		}

		// GET api/<GroupsController>/5
		[HttpGet("{id}")]
		public async Task<ActionResult<GroupDto>> GetById(Guid id)
		{
			var query = new GetGroupByIdQuery(id);

			var group = await requestSender.Send(query);

			return Ok(group);
		}

		// GET: api/<GroupsController>
		[HttpGet]
		public async Task<ActionResult<IEnumerable<GroupDto>>> ListGroups(int limit, int offset)
		{
			var query = new ListGroupsQuery(limit, offset);

			var groups = await requestSender.Send(query);

			return Ok(groups);
		}

		// PUT api/<GroupsController>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}
	}
}