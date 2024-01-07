using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebAPI.Domain.Common;

namespace WebAPI.Domain.Entities
{
    public class LoginModel : Entity
	{
		public string Username { get; set; }
		public string Password { get; set; }
	}
}
