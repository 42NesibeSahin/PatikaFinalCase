using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Application.DTOs
{
	public class TransferDto
	{
        public int ID { get; set; }
        public string ToAccount { get; set; }                                // Alıcı hesap numarası.
		public decimal Amount { get; set; }                                  // Transfer miktarı.
		public string Status { get; set; }                                   // Örneğin, "Completed", "Pending", "Failed"
		public int AccountID { get; set; }                                   // Gönderici ID
	}

	public class TransferEkleDto
	{

		public string ToAccount { get; set; }                               
		public decimal Amount { get; set; }                                   
		public string Status { get; set; }
		public int AccountID { get; set; }
	}
}
