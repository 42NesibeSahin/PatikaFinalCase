using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Application.DTOs
{ 
    public class TicketDto
	{
        public int ID { get; set; }
        public string Issue { get; set; }                 // Sorunun detayı.
		public string Priority { get; set; }              // Öncelik durumu. Örneğin, "Low", "Medium", "High"
		public string Status { get; set; }                // Talebin durumu Örneğin, "Open", "In Progress", "Resolved"
		public int UserID { get; set; }                   // Kullanıcı ID'si.
	}
	public class TicketEkleDto
	{
		public string Issue { get; set; }                 // Sorunun detayı.
		public string Priority { get; set; }              // Öncelik durumu. Örneğin, "Low", "Medium", "High"
		public string Status { get; set; }                // Talebin durumu Örneğin, "Open", "In Progress", "Resolved"
		public int UserID { get; set; }                   // Kullanıcı ID'si.
	}
}
