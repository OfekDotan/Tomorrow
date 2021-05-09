using System;
using System.ComponentModel.DataAnnotations;

namespace Tomorrow.Web.Client.Models
{
	public class Todo
	{
		public string Id { get; set; }

		[StringLength(256, ErrorMessage = "Name is too long")]
		[Required]
		public string Name { get; set; }

		[Range(0, 10)]
		public int Priority { get; set; }
	}
}