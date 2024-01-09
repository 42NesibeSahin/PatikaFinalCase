using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Application.DTOs
{
    public class AccountDto
    {
        public int ID { get; set; }
        public decimal Balance { get; set; }                              // Hesap bakiyesi.
        public string AccountNumber { get; set; }                         // Hesap numarası.
        public decimal MinimumBalance { get; set; }                       // Hesap için gerekli minimum bakiye.
        public string UserID { get; set; }
    }
	public class AccountEkleDto
	{
		public decimal Balance { get; set; }                             
		//public string AccountNumber { get; set; }                         
		public decimal MinimumBalance { get; set; }                       
		public string UserID { get; set; }
	}

	public class AccountEkleDtoPut
	{
		public decimal Balance { get; set; }                              
		//public string AccountNumber { get; set; } 
	}
}
