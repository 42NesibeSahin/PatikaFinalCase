using WebAPI.Domain.Common;

namespace WebAPI.Domain.Entities
{
	public  class RegisterModel : Entity
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public string Username { get; set; }
	}
}
