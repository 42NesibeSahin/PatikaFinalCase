using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using WebAPI.Domain.Common;

namespace WebAPI.Domain.Entities
{
	//Kredi İşlemleri
	public class Loan:Entity
	{
		public decimal Amount { get; set; }                       // Kredi miktarı.
		public string ApprovalStatus { get; set; }                // Onay durumu  Örneğin, "Pending", "Approved", "Rejected"
		public string PaymentPlan { get; set; }                   // Ödeme planı detayları
		public int AccountID { get; set; }                           // Kredi başvurusu yapan kullanıcı ID'si.

		[ForeignKey("AccountID")]
		public Account Account { get; set; }
	}
}

