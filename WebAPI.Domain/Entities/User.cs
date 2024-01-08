using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPI.Domain.Common;

namespace WebAPI.Domain.Entities
{
	//Kullanıcı Yönetimi
	public class User: IdentityUser<int>,IEntity
	{
        public string FirstName { get; set; }                                // Kullanıcı adı.
		public string LastName { get; set; }                                 // Kullanıcı adı.

        public int RoleId { get; set; }

		[ForeignKey("RoleId")]
		public UserRole UserRole { get; set; }

		//public string Email { get; set; }                                   // E-posta adresi.
		//public string PasswordHash { get; set; }                            // Kullanıcının şifresinin hash'lenmiş hali.
		//public string Role { get; set; }                                    // Rolü (admin, user, auditor).
		//public string AccessToken { get; set; }                             // JWT tabanlı erişim token'ı.
		
		public List<Account> Account { get; set; }
	
		
		public List<Ticket> Ticket { get; set; }

	}
}
