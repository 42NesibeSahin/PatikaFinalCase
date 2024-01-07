using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Domain.Entities;
using WebAPI.Application.DTOs;
namespace WebAPI.Application.Common.Mappings
{
	public class TicketMapping:Profile
	{
        public TicketMapping()
        {
			CreateMap<Ticket, TicketDto>().ReverseMap();
			CreateMap<Ticket, TicketEkleDto>().ReverseMap();
			CreateMap<TicketDto, TicketEkleDto>().ReverseMap();
		}
    }
}
