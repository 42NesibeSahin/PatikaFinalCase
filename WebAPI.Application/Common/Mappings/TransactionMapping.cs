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
	public class TransactionMapping:Profile
	{
        public TransactionMapping()
        {
			CreateMap<Transaction, TransactionDto>().ReverseMap();
			CreateMap<Transaction, TransactionEkleDto>().ReverseMap();
			CreateMap<TransactionDto, TransactionEkleDto>().ReverseMap();
		}
    }
}
