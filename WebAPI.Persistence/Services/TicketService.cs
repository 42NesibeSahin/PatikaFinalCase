using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WebAPI.Application.DTOs;
using WebAPI.Application.Interfaces.Repositories;
using WebAPI.Application.Interfaces.Services;
using WebAPI.Domain.Entities;

namespace WebAPI.Persistence.Services
{
    public class TicketService:ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
		private readonly IMapper _mapper;

		public TicketService(ITicketRepository ticketRepository, IMapper mapper)
        {
            _ticketRepository = ticketRepository;
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		public async Task<IEnumerable<TicketDto>> GetAll(string sortField = "Priority", string sortOrder = "asc", string Priority = null)
		{
			var query = _ticketRepository.AsQueryable();

			// Filtreleme
			if (!string.IsNullOrEmpty(Priority))
			{
				query = query.Where(u => u.Priority.Contains(Priority));
			}
			// Sorting
			if (!string.IsNullOrEmpty(sortField))
			{
				query = sortOrder.ToLower() == "asc"
					? query.OrderBy(u => EF.Property<object>(u, sortField))
					: query.OrderByDescending(u => EF.Property<object>(u, sortField));
			}

			var tickets = await query.ToListAsync();
			return _mapper.Map<IEnumerable<TicketDto>>(tickets);
		}

		public async Task<TicketDto> CreateTicketAsync(TicketEkleDto ekleDto)
		{
			Ticket ticket = _mapper.Map<Ticket>(ekleDto);
			await _ticketRepository.AddAsync(ticket);
			await _ticketRepository.Save();
			return _mapper.Map<TicketDto>(ticket);
			
		}

		public async Task<TicketDto> GetByIDAsync(int id)
		{
			var ticket = await _ticketRepository.GetWhereQuery(x => x.Id == id).FirstOrDefaultAsync();

			if (ticket == null)
			{
				throw new KeyNotFoundException("Ticket not found");
			}

			return _mapper.Map<TicketDto>(ticket);
		}

		public async Task UpdateTicketAsync(int ticketId, TicketEkleDto ekleDto)
		{
			if (ticketId <= 0)
			{
				throw new ArgumentException("Invalid Ticket ID");
			}

			var ticketToUpdate = await _ticketRepository.GetByIDAsync(ticketId);
			if (ticketToUpdate == null)
			{
				throw new KeyNotFoundException("Ticket not found");
			}

			_mapper.Map(ekleDto, ticketToUpdate);
			await _ticketRepository.UpdateAsync(ticketToUpdate);
			await _ticketRepository.Save();
		}
	}
}
