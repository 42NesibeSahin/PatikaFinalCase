using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Application.DTOs
{
	public class TransactionDto
	{
        public int ID { get; set; }
        public decimal Amount { get; set; }                                   // İşlem miktarı.
		public string Type { get; set; }                                      // İşlem türü  "deposit" veya "withdraw"
		public DateTime Date { get; set; }                                    // İşlem tarihi. 
		public int AccountID { get; set; }                                    // İşlemi yapan kullanıcı ID'si.

	}
	public class TransactionEkleDto
	{
		public decimal Amount { get; set; }                                   // İşlem miktarı.
		public int AccountID { get; set; }                                       // İşlemi yapan kullanıcı ID'si.
		public string Type { get; set; }                                      // İşlem türü  "deposit" veya "withdraw"
		public DateTime Date { get; set; }

	}
}
