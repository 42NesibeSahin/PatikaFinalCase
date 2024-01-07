using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Application.DTOs;

namespace WebAPI.Application.Interfaces.Services
{
    public interface ITicketService
    {
		Task<IEnumerable<TicketDto>> GetAll(string sortField = "Priority", string sortOrder = "asc");
		Task<TicketDto> CreateTicketAsync(TicketEkleDto ekleDto);
		Task<TicketDto> GetByIDAsync(int id);
		Task UpdateTicketAsync(int ticketId, TicketEkleDto ekleDto);
	}
}
