using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using WebAPI.Domain.Common;

namespace WebAPI.Domain.Entities
{
	//Otomatik Ödemeler
	public class AutoPayment:Entity
	{
		public decimal Amount { get; set; }                         // Ödeme miktarı.
		public DateTime ScheduledDate { get; set; }                 // Planlanan ödeme tarihi.
		public string Status { get; set; }                          // Ödeme durumu. Örneğin, "Active", "Cancelled"
		public int AccountID { get; set; }                             // Kullanıcı ID'si.

		[ForeignKey("AccountID")]
		public Account Account { get; set; }


	}
}
