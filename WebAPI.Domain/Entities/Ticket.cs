using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPI.Domain.Common;

namespace WebAPI.Domain.Entities
{
	//Destek Talepleri
	public class Ticket:Entity
	{
		public string Issue { get; set; }                 // Sorunun detayı.
		public string Priority { get; set; }              // Öncelik durumu. Örneğin, "Low", "Medium", "High"
		public string Status { get; set; }                // Talebin durumu Örneğin, "Open", "In Progress", "Resolved"
		public int UserID { get; set; }                   // Kullanıcı ID'si.

		[ForeignKey("UserID")]
		public User User { get; set; }
	}
}
