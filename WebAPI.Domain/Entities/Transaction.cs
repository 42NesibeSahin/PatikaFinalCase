using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPI.Domain.Common;

namespace WebAPI.Domain.Entities
{
	//Para Yatırma ve Çekme
	public class Transaction:Entity
	{
		public decimal Amount { get; set; }                                   // İşlem miktarı.
		public string Type { get; set; }                                      // İşlem türü  "deposit" veya "withdraw"
		public DateTime Date { get; set; }                                    // İşlem tarihi. 
		public int AccountID { get; set; }                                    // İşlemi yapan kullanıcı ID'si.

		[Timestamp]
		public byte[] RowVersion { get; set; }

		[ForeignKey("AccountID")]
		public Account Account { get; set; }
	}
}
