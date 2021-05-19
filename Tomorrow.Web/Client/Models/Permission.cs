using System.ComponentModel.DataAnnotations;

namespace Tomorrow.Web.Client.Models
{
	public class Permission
	{
		[EmailAddress]
		public string Email { get; set; }
	}
}