using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Application.DTOs;
using WebAPI.Domain.Entities;
namespace WebAPI.Application.Common.Mappings
{
	public class TransferMapping:Profile
	{
        public TransferMapping()
        {
			CreateMap<Transfer, TransferDto>().ReverseMap();
			CreateMap<Transfer, TransferEkleDto>().ReverseMap();
			CreateMap<TransferDto, TransferEkleDto>().ReverseMap();
		}
    }
}
