﻿using System.ComponentModel.DataAnnotations;

namespace Tomorrow.Web.Client.Models
{
	public class Group
	{
		public string Id { get; set; }

		[StringLength(256, ErrorMessage = "Name is too long")]
		public string Name { get; set; }
	}
}