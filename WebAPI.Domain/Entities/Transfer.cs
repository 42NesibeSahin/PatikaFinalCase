using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPI.Domain.Common;

namespace WebAPI.Domain.Entities
{
	//Para Transferleri
	public class Transfer:Entity
	{
		
		public string ToAccount { get; set; }                                // Alıcı hesap numarası.
		public decimal Amount { get; set; }                                  // Transfer miktarı.
		public DateTime TransferDate { get; set; }                           // Tarih
		public string Status { get; set; }                                   // Örneğin, "Completed", "Pending", "Failed"
        public int AccountID { get; set; }                                     // Gönderici ID

		[ForeignKey("AccountID")]
		public Account Account { get; set; }
	}
}
