using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Domain.Common;

namespace WebAPI.Domain.Entities
{
	//Hesap Yönetimi
	public class Account:Entity
	{
		public decimal Balance { get; set; }                              // Hesap bakiyesi.
		public string AccountNumber { get; set; }                         // Hesap numarası.
		public decimal MinimumBalance { get; set; }                       // Hesap için gerekli minimum bakiye.
		public int UserID { get; set; }                                   // Kullanıcı ID'si.

		[ForeignKey("UserID")]
		public User User { get; set; }


		public List<Transaction> Transaction { get; set; }
		public List<AutoPayment> AutoPayment { get; set; }
		public List<Transfer> Transfer { get; set; }
		public List<Loan> Loan { get; set; }
	}
}
